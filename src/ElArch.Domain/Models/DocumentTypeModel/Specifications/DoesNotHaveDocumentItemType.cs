using System;
using System.Collections.Generic;
using System.Linq;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.Entities;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class DoesNotHaveDocumentItemType : Specification<DocumentTypeAggregate>
    {
        private readonly DocumentItemType _documentItemType;

        public DoesNotHaveDocumentItemType(DocumentItemType documentItemType)
        {
            _documentItemType = documentItemType ?? throw new ArgumentNullException(nameof(documentItemType));
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if (aggregate.State.DocumentItemTypes.ContainsKey(_documentItemType.Name))
                yield return $"DocumentType already has ItemType with name {_documentItemType.Name}";
            if (aggregate.State.DocumentItemTypes.Values.Any(i => i.Id == _documentItemType.Id))
                yield return $"DocumentType already has ItemType with Id {_documentItemType.Id}";
        }
    }
}