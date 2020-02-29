using System;
using Akka.Actor;
using Akkatecture.Aggregates.ExecutionResults;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Commands;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using MediatR;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class RemoveDocumentTypeField : Command<DocumentTypeAggregate, DocumentTypeId>, IRequest<IExecutionResult>
    {
        public RemoveDocumentTypeField(DocumentTypeId aggregateId, FieldId fieldId) : base(aggregateId)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        public FieldId FieldId { get; }
    }

    internal sealed class RemoveDocumentTypeFieldHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, RemoveDocumentTypeField>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, RemoveDocumentTypeField command)
        {
            var specification = new AggregateIsNewSpecification().Not();
            var result =
                specification.Check(aggregate).Map(a => (DocumentTypeAggregate) a)
                    .ApplyOnLeft(a =>
                    {
                        if (a.State.Fields.TryGetValue(command.FieldId, out var field)) a.Emit(new DocumentTypeFieldRemoved(field));
                    })
                    .ToExecutionResult();
            context.Sender.Tell(result);
        }
    }

    internal sealed class RemoveDocumentTypeFieldRequestHandler : CommandRequestHandler<RemoveDocumentTypeField>
    {
        public RemoveDocumentTypeFieldRequestHandler(IActorRef aggregateManager) : base(aggregateManager)
        {
        }
    }
}