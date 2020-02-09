using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeNameChanged : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {

        public DocumentTypeNameChanged([NotNull] DocumentTypeName documentTypeName)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }
        [NotNull] public DocumentTypeName DocumentTypeName { get; }
    }
}