using System;
using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class DocumentTypeFieldRemoved : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public DocumentTypeFieldRemoved([NotNull] Field field)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        [NotNull] public Field Field { get; }
    }
}