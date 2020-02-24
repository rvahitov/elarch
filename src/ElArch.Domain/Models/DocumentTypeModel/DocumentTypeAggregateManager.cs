using Akkatecture.Aggregates;
using Akkatecture.Commands;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    public sealed class DocumentTypeAggregateManager
        : AggregateManager<DocumentTypeAggregate, DocumentTypeId, Command<DocumentTypeAggregate, DocumentTypeId>>
    {
    }
}