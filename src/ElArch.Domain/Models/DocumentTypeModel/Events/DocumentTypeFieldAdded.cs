using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeFieldAdded : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentTypeFieldAdded([NotNull] Field field)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        [NotNull] public Field Field { get; }
    }
}