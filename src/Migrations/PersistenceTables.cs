using FluentMigrator;

namespace Migrations
{
    [Migration(1)]
    public sealed class PersistenceTables : Migration
    {
        public override void Up()
        {
            IfDatabase("Postgres").Delegate(() =>
            {
                Execute.Sql(AkkaPersistencePostgresScripts.CreateJournalTable);
                Execute.Sql(AkkaPersistencePostgresScripts.CreateSnapshotTable);
                Execute.Sql(AkkaPersistencePostgresScripts.CreateMetadataTable);
            });
        }

        public override void Down()
        {
            IfDatabase("Postgres").Delegate(() =>
            {
                Execute.Sql(AkkaPersistencePostgresScripts.DeleteJournalTable);
                Execute.Sql(AkkaPersistencePostgresScripts.DeleteSnapshotTable);
                Execute.Sql(AkkaPersistencePostgresScripts.DeleteMetadataTable);
            });
        }
    }
}