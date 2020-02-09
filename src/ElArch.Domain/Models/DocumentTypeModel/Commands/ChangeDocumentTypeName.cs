using System;
using Akka.Actor;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class ChangeDocumentTypeName : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public ChangeDocumentTypeName(DocumentTypeId aggregateId, [NotNull] DocumentTypeName documentTypeName) : base(aggregateId)
        {
            DocumentTypeName = documentTypeName ?? throw new ArgumentNullException(nameof(documentTypeName));
        }

        [NotNull] public DocumentTypeName DocumentTypeName { get; }
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
                    if (a.State.DocumentTypeName != command.DocumentTypeName)
                        a.Emit(new DocumentTypeNameChanged(command.DocumentTypeName));
                })
                .ToExecutionResult();

            context.Sender.Tell(result);
        }
    }
}