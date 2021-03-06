using System;
using System.Linq;
using System.Transactions;
using Akka.Actor;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Storage.DocumentType.ReadModels;

namespace ElArch.Storage.DocumentType
{
    internal sealed partial class DocumentTypeStorageSubscriber
    {
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
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, SearchViewChanged>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, GridViewChanged>>(Handle);
                Receive<IDomainEvent<DocumentTypeAggregate, DocumentTypeId, CardViewChanged>>(Handle);
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeCreated> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
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
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeNameChanged> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                model.Name = domainEvent.AggregateEvent.DocumentTypeName;
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldAdded> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
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
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, DocumentTypeFieldRemoved> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var fieldReadModel = context.Find<FieldReadModel>(domainEvent.AggregateEvent.Field.FieldId, domainEvent.AggregateIdentity);
                if (fieldReadModel == null) return;
                model.Fields.Remove(fieldReadModel);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, SearchViewChanged> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var oldSearchViewModels = (
                    from dt in context.Set<DocumentTypeReadModel>()
                    from dv in dt.DocumentViews.OfType<SearchViewReadModel>()
                    where dt.Id == domainEvent.AggregateIdentity
                    select dv
                );
                foreach (var documentView in oldSearchViewModels)
                {
                    model.DocumentViews.Remove(documentView);
                }
                context.SaveChanges();

                if (domainEvent.AggregateEvent.SearchView == null)
                {
                    unitOfWork.Complete();
                    return;
                }

                var newDocumentView = DocumentViewReadModel.FromDomainModel(domainEvent.AggregateEvent.SearchView);
                newDocumentView.DocumentTypeId = model.Id;
                model.DocumentViews.Add(newDocumentView);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, GridViewChanged> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var oldSearchViewModels = (
                    from dt in context.Set<DocumentTypeReadModel>()
                    from dv in dt.DocumentViews.OfType<GridViewReadModel>()
                    where dt.Id == domainEvent.AggregateIdentity
                    select dv
                );
                foreach (var documentView in oldSearchViewModels)
                {
                    model.DocumentViews.Remove(documentView);
                }

                context.SaveChanges();
                if (domainEvent.AggregateEvent.GridView == null)
                {
                    unitOfWork.Complete();
                    return;
                }

                var newDocumentView = DocumentViewReadModel.FromDomainModel(domainEvent.AggregateEvent.GridView);
                newDocumentView.DocumentTypeId = model.Id;
                model.DocumentViews.Add(newDocumentView);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
                unitOfWork.Complete();
            }

            private void Handle(IDomainEvent<DocumentTypeAggregate, DocumentTypeId, CardViewChanged> domainEvent)
            {
                using var unitOfWork = new TransactionScope(TransactionScopeOption.RequiresNew);
                using var context = _contextFactory();
                var model = context.Find<DocumentTypeReadModel>(domainEvent.AggregateIdentity);
                if (model == null) return;
                var oldSearchViewModels = (
                    from dt in context.Set<DocumentTypeReadModel>()
                    from dv in dt.DocumentViews.OfType<CardViewReadModel>()
                    where dt.Id == domainEvent.AggregateIdentity
                    select dv
                );
                foreach (var documentView in oldSearchViewModels)
                {
                    model.DocumentViews.Remove(documentView);
                }

                context.SaveChanges();
                if (domainEvent.AggregateEvent.CardView == null)
                {
                    unitOfWork.Complete();
                    return;
                }

                var newDocumentView = DocumentViewReadModel.FromDomainModel(domainEvent.AggregateEvent.CardView);
                newDocumentView.DocumentTypeId = model.Id;
                model.DocumentViews.Add(newDocumentView);
                model.ModificationTime = domainEvent.Timestamp;
                model.Version += 1;
                context.SaveChanges();
                unitOfWork.Complete();
                
            }

            public static object GetConsistentHash(object o) => o switch
            {
                IDomainEvent de => de.GetIdentity(),
                _ => null
            };
        }
    }
}