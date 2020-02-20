using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Migrations;

namespace WebApplication.Infrastructure
{
    public static class MigrationBootstrapper
    {
        public static void ConfigureMigrations(HostBuilderContext builderContext, IServiceCollection services)
        {
            var connectionString = builderContext.Configuration.GetConnectionString("Default");
            var dbType = builderContext.Configuration.GetValue<string>("Migrations:DbType");
            if (string.IsNullOrEmpty(dbType)) return;
            var usePostgres = string.Equals(dbType, "Postgres", StringComparison.OrdinalIgnoreCase);
            var useSqlServer = string.Equals(dbType, "SqlServer", StringComparison.OrdinalIgnoreCase);
            if (!usePostgres && !useSqlServer) return;
            services.AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder =>
                {
                    if (usePostgres)
                    {
                        runnerBuilder.AddPostgres();
                    }
                    else if (useSqlServer)
                    {
                        runnerBuilder.AddSqlServer();
                    }
                    else
                    {
                        return;
                    }

                    runnerBuilder.WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(FieldTable).Assembly)
                        .For.Migrations();
                    services.AddLogging(lb => lb.AddFluentMigratorConsole());
                });
        }
    }
}