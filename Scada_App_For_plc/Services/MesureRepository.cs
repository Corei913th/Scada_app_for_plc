using Npgsql;

namespace Scada_App_For_plc.Services;

/// <summary>Une ligne de la table « mesures » : les 3 vitesses lues dans D3100..D3102 à un instant donné.</summary>
public sealed record Mesure(int Id, DateTime Horodatage, int Moteur1, int Moteur2, int Moteur3);

/// <summary>Archivage des vitesses moteurs dans PostgreSQL (requêtes paramétrées).</summary>
public sealed class MesureRepository
{
    private readonly string _connectionString;
    private bool _ready;

    public MesureRepository(string connectionString) => _connectionString = connectionString;

    /// <summary>Crée la table au premier usage si elle n'existe pas (la base « scada » elle-même doit exister).</summary>
    public async Task EnsureCreatedAsync()
    {
        if (_ready)
            return;

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            """
            CREATE TABLE IF NOT EXISTS mesures (
                id          SERIAL PRIMARY KEY,
                horodatage  TIMESTAMPTZ NOT NULL DEFAULT now(),
                moteur1     INTEGER NOT NULL,
                moteur2     INTEGER NOT NULL,
                moteur3     INTEGER NOT NULL
            )
            """, conn);
        await cmd.ExecuteNonQueryAsync();
        _ready = true;
    }

    /// <summary>Enregistre une mesure : les 3 vitesses lues dans l'automate. La date est posée par PostgreSQL (now()).</summary>
    public async Task InsertAsync(IReadOnlyList<int> speeds)
    {
        if (speeds.Count < 3)
            throw new ArgumentException("Trois vitesses sont attendues.", nameof(speeds));

        await EnsureCreatedAsync();
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "INSERT INTO mesures (moteur1, moteur2, moteur3) VALUES (@m1, @m2, @m3)", conn);
        cmd.Parameters.AddWithValue("m1", speeds[0]);
        cmd.Parameters.AddWithValue("m2", speeds[1]);
        cmd.Parameters.AddWithValue("m3", speeds[2]);
        await cmd.ExecuteNonQueryAsync();
    }

    /// <summary>Renvoie les <paramref name="limit"/> dernières mesures, la plus récente en premier.</summary>
    public async Task<List<Mesure>> GetLatestAsync(int limit = 1000)
    {
        await EnsureCreatedAsync();
        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand(
            "SELECT id, horodatage, moteur1, moteur2, moteur3 FROM mesures ORDER BY id DESC LIMIT @limit", conn);
        cmd.Parameters.AddWithValue("limit", limit);

        var result = new List<Mesure>();
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            result.Add(new Mesure(
                reader.GetInt32(0),
                reader.GetFieldValue<DateTime>(1).ToLocalTime(),
                reader.GetInt32(2),
                reader.GetInt32(3),
                reader.GetInt32(4)));
        }

        return result;
    }
}
