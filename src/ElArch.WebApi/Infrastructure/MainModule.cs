using Autofac;
using ElArch.Domain;
using ElArch.Storage;
using ElArch.WebApi.Controllers.DocumentTypes;

namespace ElArch.WebApi.Infrastructure
{
    public sealed class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ActorSystemModule());
            builder.RegisterModule(new DomainModule());
            builder.RegisterModule(new StorageModule());
            builder.RegisterModule(new DataLayerModule());
            builder.RegisterModule(new MediatorModule());
            builder.RegisterModule(new DocumentTypeControllerModule());
        }
    }
}