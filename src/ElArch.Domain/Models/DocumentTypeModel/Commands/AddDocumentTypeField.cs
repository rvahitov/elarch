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
    public sealed class AddDocumentTypeField : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public AddDocumentTypeField(DocumentTypeId aggregateId, [NotNull] Field field) : base(aggregateId)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        [NotNull] public Field Field { get; }
    }

    [UsedImplicitly]
    internal sealed class AddDocumentTypeFieldHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, AddDocumentTypeField>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, AddDocumentTypeField command)
        {
            var specification = new AggregateIsNewSpecification().Not()
                .And(new DocumentTypeNotContainsFieldSpecification(command.Field));
            var result = specification.Check(aggregate)
                .ApplyOnLeft(a => a.Emit(new DocumentTypeFieldAdded(command.Field)))
                .ToExecutionResult();

            context.Sender.Tell(result);
        }
    }
}