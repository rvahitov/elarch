using Autofac.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApplication.Infrastructure;

namespace WebApplication
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var config = host.Services.GetRequiredService<IConfiguration>();
            var runMigrations = config.GetValue<bool?>("RunMigrations");
            if (runMigrations == true)
            {
                using var scope = host.Services.CreateScope();
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
                return;
            }

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices(MigrationBootstrapper.ConfigureMigrations)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}