using Autofac;
using ElArch.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ElArch.WebApi.Infrastructure
{
    public sealed class DataLayerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var config = c.Resolve<IConfiguration>();
                    var connectionString = config.GetConnectionString("DomainStorage");
                    var contextOptionsBuilder = new DbContextOptionsBuilder<ElArchContext>();
                    contextOptionsBuilder.UseSqlServer(connectionString);
                    return contextOptionsBuilder.Options;
                })
                .SingleInstance();
        }
    }
}