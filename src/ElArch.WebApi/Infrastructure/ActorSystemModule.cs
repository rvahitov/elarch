using System.IO;
using System.Text;
using Akka.Actor;
using Akka.Configuration;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ElArch.WebApi.Infrastructure
{
    public sealed class ActorSystemModule : Module
    {
        private static Config ReadConfiguration(IHostEnvironment environment)
        {
            var sb = new StringBuilder();
            var appSettings = Path.Combine(environment.ContentRootPath, @"appsettings.json");
            var envAppSettings = Path.Combine(environment.ContentRootPath, @$"appsettings.{environment.EnvironmentName}.json");
            var persistence = Path.Combine(environment.ContentRootPath, @"akka_persistence_sqlserver.hocon");
            if (File.Exists(appSettings))
            {
                sb.AppendLine(File.ReadAllText(appSettings));
            }

            if (File.Exists(envAppSettings))
            {
                sb.AppendLine(File.ReadAllText(envAppSettings));
            }

            if (File.Exists(persistence))
            {
                sb.AppendLine(File.ReadAllText(persistence));
            }

            return sb.Length > 0 ? ConfigurationFactory.ParseString(sb.ToString()) : Config.Empty;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var environment = c.Resolve<IWebHostEnvironment>();
                    var config = ReadConfiguration(environment);
                    return ActorSystem.Create("el-arch", config);
                })
                .As<ActorSystem>()
                .SingleInstance();
        }
    }
}