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
using JetBrains.Annotations;
using MediatR;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class ChangeDocumentTypeName : Command<DocumentTypeAggregate, DocumentTypeId>, IRequest<IExecutionResult>
    {
        public ChangeDocumentTypeName(DocumentTypeId aggregateId, DocumentTypeName documentTypeName) : base(aggregateId)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }

        public DocumentTypeName DocumentTypeName { get; }
    }

    [UsedImplicitly]
    internal sealed class ChangeDocumentTypeNameHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, ChangeDocumentTypeName>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, ChangeDocumentTypeName command)
        {
            var specification = new AggregateIsNewSpecification().Not();

            var result = specification.Check(aggregate)
                .Map(a => (DocumentTypeAggregate) a)
                .ApplyOnLeft(a =>
                {
                    if (command.DocumentTypeName.Equals(a.State.DocumentTypeName)) return;
                    a.Emit(new DocumentTypeNameChanged(command.DocumentTypeName));
                })
                .ToExecutionResult();
            context.Sender.Tell(result);
        }
    }

    internal sealed class ChangeDocumentTypeRequestNameHandler : CommandRequestHandler<ChangeDocumentTypeName>
    {
        public ChangeDocumentTypeRequestNameHandler(IActorRef aggregateManager) : base(aggregateManager)
        {
        }
    }
}