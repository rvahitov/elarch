using Akka.Actor;
using Autofac;
using ElArch.Domain.Models.DocumentTypeModel.Commands;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    public sealed class DocumentTypeModule : Module
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

            builder.Register(c => new CreateDocumentTypeRequestHandler(c.ResolveKeyed<IActorRef>(typeof(DocumentTypeAggregateManager))))
                .AsImplementedInterfaces();
        }
    }
}