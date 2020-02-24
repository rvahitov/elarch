using System;
using Akka.Actor;
using Akka.Routing;
using Akkatecture.Aggregates;
using Akkatecture.Subscribers;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Storage.DocumentType.ReadModels;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage.DocumentType
{
    internal sealed class DocumentTypeStorageSubscriber : DomainEventSubscriber,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>
    {
        private readonly Props _handlerProps;
        private IActorRef _handler;

        public DocumentTypeStorageSubscriber(DbContextOptions<ElArchContext> dbContextOptions)
        {
            _handlerProps =
                Props.Create(() => new Handler(() => new ElArchContext(dbContextOptions)))
                    .WithRouter(new ConsistentHashingPool(10).WithHashMapping(Handler.GetConsistentHash));
        }

        protected override void PreStart()
        {
            _handler = Context.ActorOf(_handlerProps);
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        private sealed class Handler : ReceiveActor
        {
            private readonly Func<ElArchContext> _contextFactory;

            public Handler(Func<ElArchContext> contextFactory)
            {
                _contextFactory = contextFactory;
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>>(Handle);
            }

            public void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
            {
                var model = new DocumentTypeReadModel
                {
                    AggregateId = domainEvent.AggregateIdentity,
                    Name = domainEvent.AggregateEvent.DocumentTypeName,
                    CreationTime = domainEvent.Timestamp,
                    ModificationTime = domainEvent.Timestamp,
                    Version = 1
                };
                using var context = _contextFactory();
                context.Add(model);
                context.SaveChanges();
            }

            public static object GetConsistentHash(object o) => o switch
            {
                IDomainEvent de => de.GetIdentity(),
                _ => null
            };
        }
    }
}