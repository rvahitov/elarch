using System;
using Akka.Actor;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Entities;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.Specifications;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class AddDocumentItemType : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public AddDocumentItemType(DocumentTypeId aggregateId, DocumentItemType documentItemType) : base(aggregateId)
        {
            DocumentItemType = documentItemType ?? throw new ArgumentNullException(nameof(documentItemType));
        }

        public DocumentItemType DocumentItemType { get; }
    }

    [UsedImplicitly]
    internal sealed class AddDocumentItemTypeHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, AddDocumentItemType>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, AddDocumentItemType command)
        {
            var specification = new AggregateIsNewSpecification().Not()
                .And(new DoesNotHaveDocumentItemType(command.DocumentItemType));
            var result = specification.Check(aggregate)
                .ApplyOnLeft(a => a.Emit(new DocumentItemTypeAdded(command.DocumentItemType)))
                .ToExecutionResult();
            context.Sender.Tell(result);
        }
    }
}