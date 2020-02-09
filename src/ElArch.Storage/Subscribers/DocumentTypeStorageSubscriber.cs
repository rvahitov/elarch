using Akka.Actor;
using Akka.Routing;
using Akkatecture.Aggregates;
using Akkatecture.Subscribers;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Storage.ReadModels;

namespace ElArch.Storage.Subscribers
{
    internal sealed class DocumentTypeStorageSubscriber : DomainEventSubscriber,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged>
    {
        private readonly IElArchStorageContextFactory _contextFactory;
        private IActorRef _handler;

        public DocumentTypeStorageSubscriber(IElArchStorageContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected override void PreStart()
        {
            var props = Props.Create(() => new Handler(_contextFactory))
                .WithRouter(new ConsistentHashingPool(100).WithHashMapping(Hasher));
            _handler = Context.ActorOf(props, "handler");
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
        {
            _handler.Forward(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged> domainEvent)
        {
            _handler.Forward(domainEvent);
            return true;
        }

        private static object Hasher(object msg) => msg switch
        {
            IDomainEvent de => de.GetIdentity().Value,
            _ => null
        };

        private sealed class Handler : ReceiveActor
        {
            private readonly IElArchStorageContextFactory _contextFactory;

            public Handler(IElArchStorageContextFactory contextFactory)
            {
                _contextFactory = contextFactory;
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged>>(Handle);
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
            {
                using var context = _contextFactory.Create();
                var readModel = new DocumentTypeReadModel(domainEvent.AggregateIdentity)
                {
                    Name = domainEvent.AggregateEvent.DocumentTypeName,
                    Version = 1
                };
                readModel.CreationTime = readModel.ModificationTime = domainEvent.Timestamp;
                context.Add(readModel);
                context.SaveChanges();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged> domainEvent)
            {
                using var context = _contextFactory.Create();
                var readModel = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (readModel == null) return;
                readModel.Name = domainEvent.AggregateEvent.DocumentTypeName;
                readModel.ModificationTime = domainEvent.Timestamp;
                readModel.Version += 1;
                context.SaveChanges();
            }
        }
    }
}