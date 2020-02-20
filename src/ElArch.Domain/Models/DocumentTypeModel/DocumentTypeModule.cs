using Akka.Actor;
using Autofac;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    internal sealed class DocumentTypeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var props = Props.Create(() => new DocumentTypeAggregateManager());
                    return c.Resolve<ActorSystem>().ActorOf(props, "document-type-manager");
                })
                .Keyed<IActorRef>(typeof(DocumentTypeAggregateManager))
                .SingleInstance();

            builder.Register(c => new DocumentTypeManager(c.ResolveKeyed<IActorRef>(typeof(DocumentTypeAggregateManager))))
                .As<IDocumentTypeManager>();
        }
    }
}