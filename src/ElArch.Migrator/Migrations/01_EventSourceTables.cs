using FluentMigrator;

namespace ElArch.Migrator.Migrations
{
    [Migration(1)]
    public sealed class EventSourceTables : Migration
    {
        public override void Up()
        {
            IfDatabase("SqlServer").Delegate(() =>
            {
                Execute.Sql(SqlServerScripts.CreateJournalTable);
                Execute.Sql(SqlServerScripts.CreateSnapshotTable);
                Execute.Sql(SqlServerScripts.CreateMetadataTable);
            });

            IfDatabase("Postgres").Delegate(() =>
            {
                Execute.Sql(PostgresScripts.CreateJournalTable);
                Execute.Sql(PostgresScripts.CreateSnapshotTable);
                Execute.Sql(PostgresScripts.CreateMetadataTable);
            });
        }

        public override void Down()
        {
            IfDatabase("SqlServer").Delegate(() =>
            {
                Execute.Sql(SqlServerScripts.DeleteJournalTable);
                Execute.Sql(SqlServerScripts.DeleteSnapshotTable);
                Execute.Sql(SqlServerScripts.DeleteMetadataTable);
            });

            IfDatabase("Postgres").Delegate(() =>
            {
                Execute.Sql(PostgresScripts.DeleteJournalTable);
                Execute.Sql(PostgresScripts.DeleteSnapshotTable);
                Execute.Sql(PostgresScripts.DeleteMetadataTable);
            });
        }
    }
}