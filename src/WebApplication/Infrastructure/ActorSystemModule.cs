using System.IO;
using Akka.Actor;
using Akka.Configuration;
using Autofac;
using Microsoft.AspNetCore.Hosting;

namespace WebApplication.Infrastructure
{
    public sealed class ActorSystemModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var env = c.Resolve<IWebHostEnvironment>();
                    var config = LoadConfig(env);
                    return ActorSystem.Create("ElArch", config);
                })
                .As<ActorSystem>()
                .AsImplementedInterfaces()
                .SingleInstance();
        }


        private static Config LoadConfig(IWebHostEnvironment env)
        {
            var configFilePath = Path.Combine(env.ContentRootPath, "akka.hocon");
            if(File.Exists(configFilePath) == false)
                return Config.Empty;

            var content = File.ReadAllText(configFilePath);
            return ConfigurationFactory.ParseString(content);
        }
    }
}