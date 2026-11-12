namespace Scada_App_For_plc.Services;

/// <summary>Conteneur des services partagés par toutes les vues.</summary>
public sealed class AppServices : IDisposable
{
    public AppSettings Settings { get; }

    /// <summary>Liaison Modbus TCP avec l'automate (une seule instance : une seule connexion pour toute l'application).</summary>
    public PlcService Plc { get; }

    /// <summary>Accès à la base PostgreSQL.</summary>
    public MesureRepository Repo { get; }

    private AppServices(AppSettings settings)
    {
        Settings = settings;
        Plc = new PlcService(settings.Plc);
        Repo = new MesureRepository(settings.Database.ConnectionString);
    }

    public static AppServices Create() => new(AppSettings.Load());

    public void Dispose() => Plc.Dispose();
}
