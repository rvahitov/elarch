#nullable enable
using System;
using System.Collections.Generic;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class DocumentTypeContainsFieldSpecification : Specification<DocumentTypeAggregate>
    {
        private readonly FieldId _fieldId;

        public DocumentTypeContainsFieldSpecification( FieldId fieldId)
        {
            _fieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if (aggregate == null) throw new ArgumentNullException(nameof(aggregate));
            if(aggregate.State.Fields.ContainsKey(_fieldId)) yield break;
            yield return $"Document type {aggregate.State.DocumentTypeName} does not contain field {_fieldId}";
        }
    }
}