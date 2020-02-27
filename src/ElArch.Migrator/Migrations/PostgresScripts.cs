namespace ElArch.Migrator.Migrations
{
    public static class PostgresScripts
    {
        public const string CreateJournalTable = @"
CREATE TABLE event_journal (
    ordering BIGSERIAL NOT NULL PRIMARY KEY,
    persistence_id VARCHAR(255) NOT NULL,
    sequence_nr BIGINT NOT NULL,
    is_deleted BOOLEAN NOT NULL,
    created_at BIGINT NOT NULL,
    manifest VARCHAR(500) NOT NULL,
    payload BYTEA NOT NULL,
    tags VARCHAR(100) NULL,
    serializer_id INTEGER NULL,
    CONSTRAINT event_journal_uq UNIQUE (persistence_id, sequence_nr)
        );";

        public const string CreateSnapshotTable = @"
CREATE TABLE snapshot_store (
    persistence_id VARCHAR(255) NOT NULL,
    sequence_nr BIGINT NOT NULL,
    created_at BIGINT NOT NULL,
    manifest VARCHAR(500) NOT NULL,
    payload BYTEA NOT NULL,
    serializer_id INTEGER NULL,
    CONSTRAINT snapshot_store_pk PRIMARY KEY (persistence_id, sequence_nr)
);";

        public const string CreateMetadataTable = @"
CREATE TABLE metadata (
    persistence_id VARCHAR(255) NOT NULL,
    sequence_nr BIGINT NOT NULL,
    CONSTRAINT metadata_pk PRIMARY KEY (persistence_id, sequence_nr)
);";

        public const string DeleteJournalTable = @"DROP TABLE event_journal";

        public const string DeleteSnapshotTable = @"DROP TABLE snapshot_store";

        public const string DeleteMetadataTable = @"DROP TABLE metadata";
    }
}