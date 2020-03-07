using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class GridViewChanged : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {

        public GridViewChanged([CanBeNull] GridView? gridView)
        {
            GridView = gridView;
        }
        [CanBeNull] public GridView? GridView { get; }
    }
}