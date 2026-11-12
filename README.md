# Scada_app_for_plc

Application SCADA pour automate Delta, en C# (.NET 10, WinForms). Elle pilote trois moteurs par Modbus, archive les vitesses dans PostgreSQL et les exporte vers Excel. Projet du cours *Create Your Own SCADA for PLC with C# Language*.

![Écran principal](docs/ecran-principal.png)

## Pages

| Page | Rôle |
|---|---|
| Principal | Démarrer / arrêter la machine, piloter chaque moteur, hélice animée et vitesse |
| Paramètres | Consignes de vitesse, mesures, enregistrement en base |
| Historique | Mesures PostgreSQL, export `.xlsx` |
| Info | Configuration active |

## Démarrage

Prérequis : Windows, SDK .NET 10, PostgreSQL.

1. Créer la base : `psql -U postgres -f database/create_scada_db.sql`
2. Copier `Scada_App_For_plc/appsettings.example.json` en `appsettings.json` et mettre le mot de passe PostgreSQL.
3. Lancer : `dotnet run --project Scada_App_For_plc`

## Cible : réglages `Plc` dans `appsettings.json`

| Cible | Ip | Port | Protocol | CoilOffset | RegisterOffset |
|---|---|---|---|---|---|
| Simulateur ISPSoft (DVP Simulator) | 127.0.0.1 | 10002 | AsciiOverTcp | 2048 | 4096 |
| `PlcSimulator` (ce dépôt) | 127.0.0.1 | 502 | Tcp | 0 | 0 |
| Vrai automate (ex. AS218PX) | IP de l'automate | 502 | Tcp | selon le manuel | selon le manuel |

`CoilOffset` et `RegisterOffset` sont les adresses Modbus de `M0` et `D0`. Elles dépendent de la gamme d'automate.

## Adresses automate

| Adresse | Sens | Rôle |
|---|---|---|
| M0 / M1 | PC → automate | Moteur 1 : avant / arrière |
| M2 / M3 | PC → automate | Moteur 2 : avant / arrière |
| M4 / M5 | PC → automate | Moteur 3 : avant / arrière |
| M20 | PC → automate | Marche machine |
| D3000 à D3002 | PC → automate | Consignes de vitesse |
| D3100 à D3102 | automate → PC | Vitesses réelles |

Ladder minimal (ISPSoft) :

```
Moteur n avant   : M(2n) · M20 · /M(2n+1) → Y(2n)
Moteur n arrière : M(2n+1) · M20 · /M(2n) → Y(2n+1)
Toujours ON      : MOV D3000 D3100 ; MOV D3001 D3101 ; MOV D3002 D3102
```

## Tester sans automate

```bash
dotnet run --project PlcSimulator
```

Faux automate Modbus TCP (127.0.0.1:502). Sa console affiche l'état des cases M et D et un journal de ce que le SCADA écrit.

## Structure

| Dossier | Contenu |
|---|---|
| `Scada_App_For_plc/` | Application WinForms (`Views/`, `Services/`) |
| `PlcSimulator/` | Faux automate de test |
| `database/` | Script SQL |
| `docs/` | Capture d'écran |

Technologies : NModbus, Npgsql, ClosedXML.
