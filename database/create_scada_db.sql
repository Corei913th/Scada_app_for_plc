-- À exécuter une fois avec un compte PostgreSQL ayant le droit de créer une base :
--   psql -U postgres -f create_scada_db.sql
-- (la table "mesures" est ensuite créée automatiquement par l'application si besoin)

CREATE DATABASE scada;

\connect scada

CREATE TABLE IF NOT EXISTS mesures (
    id          SERIAL PRIMARY KEY,
    horodatage  TIMESTAMPTZ NOT NULL DEFAULT now(),
    moteur1     INTEGER NOT NULL,
    moteur2     INTEGER NOT NULL,
    moteur3     INTEGER NOT NULL
);
