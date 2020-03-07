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
    public sealed class SetGridView : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public SetGridView(DocumentTypeId aggregateId, [CanBeNull] GridView? gridView) : base(aggregateId)
        {
            GridView = gridView;
        }

        [CanBeNull] public GridView? GridView { get; }
    }

    internal sealed class SetGridViewHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, SetGridView>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, SetGridView command)
        {
            var specification = new AggregateIsNewSpecification().Not()
                .And(new SetDocumentViewSpecification(command.GridView));
            var executionResult =
                specification.Check(aggregate)
                    .ApplyOnLeft(a =>
                    {
                        if (Equals(command.GridView, a.State.GridView)) return;
                        a.Emit(new GridViewChanged(command.GridView));
                    })
                    .ToExecutionResult();
            context.Sender.Tell(executionResult);
        }
    }
}