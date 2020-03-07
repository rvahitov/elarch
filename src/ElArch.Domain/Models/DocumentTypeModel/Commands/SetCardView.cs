using Akka.Actor;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class SetCardView : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public SetCardView(DocumentTypeId aggregateId, [CanBeNull] CardView? cardView) : base(aggregateId)
        {
            CardView = cardView;
        }

        [CanBeNull] public CardView? CardView { get; }
    }

    internal sealed class SetCardViewHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, SetCardView>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, SetCardView command)
        {
            var specification = new AggregateIsNewSpecification()
                .Not()
                .And(new SetDocumentViewSpecification(command.CardView));
            var executionResult = specification.Check(aggregate)
                .ApplyOnLeft(a =>
                {
                    if (Equals(a.State.CardView, command.CardView)) return;
                    a.Emit(new CardViewChanged(command.CardView));
                })
                .ToExecutionResult();
            context.Sender.Tell(executionResult);
        }
    }
}