using Akkatecture.Aggregates;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Events
{
    public sealed class CardViewChanged : AggregateEvent<DocumentTypeAggregate, DocumentTypeId>
    {
        public CardViewChanged([CanBeNull] CardView? cardView)
        {
            CardView = cardView;
        }
        [CanBeNull] public CardView? CardView { get; }
    }
}