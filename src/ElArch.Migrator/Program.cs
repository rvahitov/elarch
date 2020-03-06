using System;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ElArch.Migrator
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var configuration = BuildConfiguration(args);
            var connectionString = configuration.GetValue<string>("PostgresConnection");
            var task = configuration.GetValue<string>("task");
            var services = CreateServices(connectionString);
            using var scope = services.CreateScope();
            UpdateDatabase(task, scope.ServiceProvider);
        }

        private static IConfiguration BuildConfiguration(string[] commandLineArgs)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(@"appsettings.json", true);
            configurationBuilder.AddEnvironmentVariables();
            configurationBuilder.AddCommandLine(commandLineArgs);
            return configurationBuilder.Build();
        }

        private static IServiceProvider CreateServices(string connectionString)
        {
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(runnerBuilder =>
                {
                    runnerBuilder
                        .AddPostgres()
                        // .AddSqlServer()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Program).Assembly).For.Migrations();
                })
                .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        private static void UpdateDatabase(string task, IServiceProvider serviceProvider)
        {
            var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
            if (string.Equals("migrate", task, StringComparison.OrdinalIgnoreCase))
            {
                migrationRunner.MigrateUp();
            }
            else if (string.Equals("rollback", task, StringComparison.OrdinalIgnoreCase))
            {
                migrationRunner.RollbackToVersion(0);
            }
            else
            {
                serviceProvider.GetService<ILogger>().LogError("Unknown task {task}", task);
            }
        }
    }
}