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
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded>,
        ISubscribeTo<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved>
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

        protected override void PreStart()
        {
            _handler = Context.ActorOf(_handlerProps);
        }

        private sealed class Handler : ReceiveActor
        {
            private readonly Func<ElArchContext> _contextFactory;

            public Handler(Func<ElArchContext> contextFactory)
            {
                _contextFactory = contextFactory;
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved>>(Handle);
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
            {
                var model = new DocumentTypeReadModel
                {
                    Id = domainEvent.AggregateIdentity,
                    Name = domainEvent.AggregateEvent.DocumentTypeName,
                    CreationTime = domainEvent.Timestamp,
                    ModificationTime = domainEvent.Timestamp,
                    Version = 1
                };
                using var context = _contextFactory();
                context.Add(model);
                context.SaveChanges();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged> domainEvent)
            {
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                model.Name = domainEvent.AggregateEvent.DocumentTypeName;
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded> domainEvent)
            {
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var fieldReadModel = FieldReadModel.FromDomainModel(domainEvent.AggregateEvent.Field);
                fieldReadModel.DocumentTypeId = domainEvent.AggregateIdentity;
                fieldReadModel.CreationTime = domainEvent.Timestamp;
                model.Fields.Add(fieldReadModel);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved> domainEvent)
            {
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var fieldReadModel = context.Find<FieldReadModel>(domainEvent.AggregateEvent.Field.FieldId, domainEvent.AggregateIdentity);
                if (fieldReadModel == null) return;
                model.Fields.Remove(fieldReadModel);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
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