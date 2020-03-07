using Akka.Actor;
using Akka.Routing;
using Akkatecture.Aggregates;
using Akkatecture.Subscribers;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using Microsoft.EntityFrameworkCore;

namespace ElArch.Storage.DocumentType
{
    internal sealed partial class DocumentTypeStorageSubscriber : DomainEventSubscriber,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, SearchViewChanged>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, GridViewChanged>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, CardViewChanged>
    {
        private readonly Props _handlerProps;
        private IActorRef _handler;

        public DocumentTypeStorageSubscriber(DbContextOptions<ElArchContext> dbContextOptions)
        {
            _handlerProps =
                Props.Create(() => new Handler(() => new ElArchContext(dbContextOptions)))
                    .WithRouter(new ConsistentHashingPool(10).WithHashMapping(Handler.GetConsistentHash));
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, SearchViewChanged> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, GridViewChanged> domainEvent)
        {
            _handler.Tell(domainEvent);
            return true;
        }

        public bool Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, CardViewChanged> domainEvent)
        {
            throw new System.NotImplementedException();
        }

        protected override void PreStart()
        {
            _handler = Context.ActorOf(_handlerProps);
        }
    }
}