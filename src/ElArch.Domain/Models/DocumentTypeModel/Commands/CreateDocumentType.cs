#nullable enable
using System;
using Akka.Actor;
using Akkatecture.Commands;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class CreateDocumentType : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public CreateDocumentType(DocumentTypeId aggregateId,  DocumentTypeName documentTypeName) : base(aggregateId)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }

         public DocumentTypeName DocumentTypeName { get; }
    }

    [UsedImplicitly]
    internal sealed class CreateDocumentTypeHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, CreateDocumentType>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, CreateDocumentType command)
        {
            var specification = new AggregateIsNewSpecification();
            var result =
                specification.Check(aggregate)
                    .Map(a => (DocumentTypeAggregate) a)
                    .ApplyOnLeft(a => a.Emit(new DocumentTypeCreated(command.DocumentTypeName)))
                    .ToExecutionResult();

            context.Sender.Tell(result);
        }
    }
}