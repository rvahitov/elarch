using System;
using System.Collections.Generic;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class DocumentTypeNotContainsFieldSpecification : Specification<DocumentTypeAggregate>
    {
        private readonly Field _field;

        public DocumentTypeNotContainsFieldSpecification([NotNull] Field field)
        {
            _field = field ?? throw new ArgumentNullException(nameof(field));
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if (aggregate.State.Fields.ContainsKey(_field.FieldId))
                yield return $"Document type {aggregate.State.DocumentTypeName} already contains field {_field.FieldId}";
        }
    }
}