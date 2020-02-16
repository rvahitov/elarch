#nullable enable
using System;
using System.Collections.Immutable;
using System.Linq;
using Akka.Actor;
using Akkatecture.Aggregates;
using Akkatecture.Commands;
using Akkatecture.Extensions;
using Akkatecture.Specifications.Provided;
using ElArch.Domain.Core.Extensions;
using ElArch.Domain.Models.DocumentTypeModel.Events;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;

namespace ElArch.Domain.Models.DocumentTypeModel.Commands
{
    public sealed class UpdateDocumentTypeField : Command<DocumentTypeAggregate, DocumentTypeId>
    {
        public UpdateDocumentTypeField(DocumentTypeId aggregateId, IField field) : base(aggregateId)
        {
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public IField Field { get; }
    }

    internal sealed class UpdateDocumentTypeFieldHandler : CommandHandler<DocumentTypeAggregate, DocumentTypeId, UpdateDocumentTypeField>
    {
        public override void Handle(DocumentTypeAggregate aggregate, IActorContext context, UpdateDocumentTypeField command)
        {
            var specification = new AggregateIsNewSpecification().Not();
            var result = specification.Check(aggregate)
                .Map(a =>
                {
                    var dta = (DocumentTypeAggregate) a;
                    var events = ImmutableList<IAggregateEvent<DocumentTypeAggregate, DocumentTypeId>>.Empty;
                    if (dta.State.Fields.ContainsKey(command.Field.FieldId))
                    {
                        events = events.Add(new DocumentTypeFieldRemoved(command.Field));
                    }

                    events = events.Add(new DocumentTypeFieldAdded(command.Field));
                    return events.ToArray();
                })
                .ApplyOnLeft(aggregate.EmitAll)
                .ToExecutionResult();
            context.Sender.Tell(result);
        }
    }
}