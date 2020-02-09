using System;
using System.Collections.Generic;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class DocumentTypeContainsFieldSpecification : Specification<DocumentTypeAggregate>
    {
        private readonly FieldId _fieldId;

        public DocumentTypeContainsFieldSpecification([NotNull] FieldId fieldId)
        {
            _fieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if(aggregate.State.Fields.ContainsKey(_fieldId)) yield break;
            yield return $"Document type {aggregate.State.DocumentTypeName} does not contain field {_fieldId}";
        }
    }
}