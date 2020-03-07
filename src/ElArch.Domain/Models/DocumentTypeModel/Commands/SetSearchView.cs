#nullable enable
using Akka.Actor;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class SetSearchView : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public SetSearchView(DocumentTypeId aggregateId, SearchView? searchView) : base(aggregateId)
        {
            SearchView = searchView;
        }

        public SearchView? SearchView { get; }
    }

    internal sealed class SetSearchViewHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, SetSearchView>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, SetSearchView command)
        {
            var specification = new AggregateIsNewSpecification()
                .Not()
                .And(new SetDocumentViewSpecification(command.SearchView));
            var executionResult = specification.Check(aggregate)
                .ApplyOnLeft(a =>
                {
                    if (Equals(a.State.SearchView, command.SearchView)) return;
                    a.Emit(new SearchViewChanged(command.SearchView));
                })
                .ToExecutionResult();
            context.Sender.Tell(executionResult);
        }
    }
}