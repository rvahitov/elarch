using Autofac;
using ElArch.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WebApplication.Infrastructure
{
    public sealed class DbModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var config = c.Resolve<IConfiguration>();
                    var connectionString = config.GetConnectionString("Default");
                    var optionBuilder = new DbContextOptionsBuilder<ElArchStorageContext>();
                    optionBuilder.UseNpgsql(connectionString);
                    return optionBuilder.Options;
                })
                .SingleInstance();

            builder.Register(c => new ElArchStorageContext(c.Resolve<DbContextOptions<ElArchStorageContext>>()))
                .InstancePerLifetimeScope();
        }
    }
}