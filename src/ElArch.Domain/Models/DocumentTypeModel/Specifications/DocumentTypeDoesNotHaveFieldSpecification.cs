using System;
using System.Collections.Generic;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class DocumentTypeDoesNotHaveFieldSpecification : Specification<DocumentTypeAggregate>
    {
        private readonly FieldId _fieldId;

        public DocumentTypeDoesNotHaveFieldSpecification(FieldId fieldId)
        {
            _fieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if (aggregate.State.Fields.ContainsKey(_fieldId))
                yield return $"Document type {aggregate.State.DocumentTypeName} already has field {_fieldId}";
        }
    }
}