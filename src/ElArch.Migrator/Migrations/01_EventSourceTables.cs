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
                Execute.Sql(SqlServerScripts.CreateJournTable);
                Execute.Sql(SqlServerScripts.CreateSnapshotTable);
                Execute.Sql(SqlServerScripts.CreateMedataTable);
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
        }
    }
}