using Akka.Actor;
using Autofac;
using ElArch.Storage.DocumentType;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage
{
    public sealed class StorageModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Bootstrapper(c.Resolve<IComponentContext>())).As<IStartable>().SingleInstance();
            builder.Register(c =>
                {
                    var options = c.Resolve<DbContextOptions<ElArchContext>>();
                    var props = Props.Create(() => new DocumentTypeStorageSubscriber(options));
                    return c.Resolve<ActorSystem>().ActorOf(props, "document-type-storage");
                })
                .Keyed<IActorRef>(typeof(DocumentTypeStorageSubscriber))
                .SingleInstance();
        }

        private class Bootstrapper : IStartable
        {
            private readonly IComponentContext _componentContext;

            public Bootstrapper(IComponentContext componentContext)
            {
                _componentContext = componentContext;
            }

            public void Start()
            {
                _componentContext.ResolveKeyed<IActorRef>(typeof(DocumentTypeStorageSubscriber));
            }
        }
    }
}