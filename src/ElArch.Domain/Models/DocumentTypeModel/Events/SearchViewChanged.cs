using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class SearchViewChanged : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {

        public SearchViewChanged([CanBeNull] SearchView? searchView)
        {
            SearchView = searchView;
        }
        public SearchView? SearchView { get; }
    }
}