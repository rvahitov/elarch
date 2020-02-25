namespace ElArch.Migrator.Migrations
{
    internal static class SqlServerScripts
    {
        public const string CreateJournTable = @"
CREATE TABLE EventJournal (
  Ordering BIGINT IDENTITY(1,1) NOT NULL,
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp BIGINT NOT NULL,
  IsDeleted BIT NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Payload VARBINARY(MAX) NOT NULL,
  Tags NVARCHAR(100) NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_EventJournal PRIMARY KEY (Ordering),
  CONSTRAINT QU_EventJournal UNIQUE (PersistenceID, SequenceNr)
);";

        public const string CreateSnapshotTable = @"
CREATE TABLE SnapshotStore (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  Timestamp DATETIME2 NOT NULL,
  Manifest NVARCHAR(500) NOT NULL,
  Snapshot VARBINARY(MAX) NOT NULL,
  SerializerId INTEGER NULL
  CONSTRAINT PK_SnapshotStore PRIMARY KEY (PersistenceID, SequenceNr)
);";

        public const string CreateMedataTable = @"
CREATE TABLE Metadata (
  PersistenceID NVARCHAR(255) NOT NULL,
  SequenceNr BIGINT NOT NULL,
  CONSTRAINT PK_Metadata PRIMARY KEY (PersistenceID, SequenceNr)
);";

        public const string DeleteJournalTable = "DROP TABLE EventJournal";
        public const string DeleteSnapshotTable = "DROP TABLE SnapshotStore";
        public const string DeleteMetadataTable = "DROP TABLE Metadata";
    }
}