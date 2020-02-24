#nullable enable
using System;
using Akka.Actor;
using Akkatecture.Aggregates.ExecutionResults;
using Akkatecture.Commands;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Commands;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using MediatR;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class CreateDocumentType : Command<DocumentTypeAggregate, DocumentTypeId>,
        IRequest<IExecutionResult>
    {
        public DocumentTypeName DocumentTypeName { get; }

        public CreateDocumentType(DocumentTypeId aggregateId, DocumentTypeName documentTypeName) : base(aggregateId)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }
    }

    internal sealed class CreateDocumentTypeHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, CreateDocumentType>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, CreateDocumentType command)
        {
            var specification = new AggregateIsNewSpecification();
            var executionResult = specification.Check(aggregate)
                .Map(a => (DocumentTypeAggregate) a)
                .ApplyOnLeft(a => a.Emit(new DocumentTypeCreated(command.DocumentTypeName)))
                .ToExecutionResult();
            context.Sender.Tell(executionResult);
        }
    }

    internal sealed class CreateDocumentTypeRequestHandler : CommandRequestHandler<CreateDocumentType>
    {
        public CreateDocumentTypeRequestHandler(IActorRef aggregateManager) : base(aggregateManager)
        {
        }
    }
}