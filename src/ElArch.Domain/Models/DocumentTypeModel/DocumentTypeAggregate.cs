#nullable enable
using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.Commands;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    [UsedImplicitly]
    public sealed class DocumentTypeAggregate : AggregateRoot<DocumentTypeAggregate, DocumentTypeId, DocumentTypeAggregateState>
    {
        public DocumentTypeAggregate(DocumentTypeId id) : base(id)
        {
            if (id == null) throw new ArgumentNullException(nameof(id));
            Command<CreateDocumentType, CreateDocumentTypeHandler>();
            Command<ChangeDocumentTypeName, ChangeDocumentTypeNameHandler>();
            Command<AddDocumentTypeField, AddDocumentTypeFieldHandler>();
            Command<RemoveDocumentTypeField, RemoveDocumentTypeFieldHandler>();
        }
    }
}