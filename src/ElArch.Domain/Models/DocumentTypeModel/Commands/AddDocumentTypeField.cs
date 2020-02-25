using System;
using Akka.Actor;
using Akkatecture.Aggregates.ExecutionResults;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Commands;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.Specifications;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using MediatR;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class AddDocumentTypeField : Command<DocumentTypeAggregate, DocumentTypeId>, IRequest<IExecutionResult>
    {
        public AddDocumentTypeField(DocumentTypeId aggregateId, IField field) : base(aggregateId)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public IField Field { get; }
    }

    internal sealed class AddDocumentTypeFieldHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, AddDocumentTypeField>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, AddDocumentTypeField command)
        {
            var specification = new AggregateIsNewSpecification().Not().And(new DocumentTypeDoesNotHaveFieldSpecification(command.Field.FieldId));
            var result = specification.Check(aggregate)
                .ApplyOnLeft(a => a.Emit(new DocumentTypeFieldAdded(command.Field)))
                .ToExecutionResult();
            context.Sender.Tell(result);
        }
    }

    internal sealed class AddDocumentTypeFieldRequestHandler : CommandRequestHandler<AddDocumentTypeField>
    {
        public AddDocumentTypeFieldRequestHandler(IActorRef aggregateManager) : base(aggregateManager)
        {
        }
    }
}