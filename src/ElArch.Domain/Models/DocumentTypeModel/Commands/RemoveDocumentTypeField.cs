using System;
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
    public sealed class RemoveDocumentTypeField : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public RemoveDocumentTypeField(DocumentTypeId aggregateId, [NotNull] FieldId fieldId) : base(aggregateId)
        {
            FieldId = fieldId ?? throw new ArgumentNullException(nameof(fieldId));
        }

        [NotNull] public FieldId FieldId { get; }
    }

    [UsedImplicitly]
    internal sealed class RemoveDocumentTypeFieldHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, RemoveDocumentTypeField>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, RemoveDocumentTypeField command)
        {
            var specification = new AggregateIsNewSpecification().Not()
                .And(new DocumentTypeContainsFieldSpecification(command.FieldId));

            var result = specification.Check(aggregate)
                .ApplyOnLeft(a =>
                {
                    var field = a.State.Fields[command.FieldId];
                    a.Emit(new DocumentTypeFieldRemoved(field));
                })
                .ToExecutionResult();

            context.Sender.Tell(result);
        }
    }
}