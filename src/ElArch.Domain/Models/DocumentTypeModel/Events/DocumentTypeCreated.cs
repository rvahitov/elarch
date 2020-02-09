using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeCreated : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {

        public DocumentTypeCreated([NotNull] DocumentTypeName documentTypeName)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }
        [NotNull] public DocumentTypeName DocumentTypeName { get; }
    }
}