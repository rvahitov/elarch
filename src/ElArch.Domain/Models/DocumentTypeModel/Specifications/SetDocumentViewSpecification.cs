using System.Collections.Generic;
using System.Linq;
using Akkatecture.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Specifications
{
    public sealed class SetDocumentViewSpecification : Specification<DocumentTypeAggregate>
    {
        private readonly IDocumentView? _documentView;

        public SetDocumentViewSpecification(IDocumentView? documentView)
        {
            _documentView = documentView;
        }

        protected override IEnumerable<string> IsNotSatisfiedBecause(DocumentTypeAggregate aggregate)
        {
            if (_documentView == null) yield break;
            foreach (var viewField in _documentView.ViewFields.Where(f => aggregate.State.Fields.ContainsKey(f.FieldId) == false))
            {
                yield return $"Document type does not contains field {viewField.FieldId}";
            }
        }
    }
}