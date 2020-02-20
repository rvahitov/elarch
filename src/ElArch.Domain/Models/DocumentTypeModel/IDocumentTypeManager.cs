using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akkatecture.Aggregates.ExecutionResults;
using Akkatecture.Commands;
using JetBrains.Annotations;

namespace ElArch.Domain.Models.DocumentTypeModel
{
    public interface IDocumentTypeManager
    {
        Task<IExecutionResult> ExecuteCommand(Command<DocumentTypeAggregate, DocumentTypeId> command, CancellationToken cancellation = default);
    }

    internal sealed class DocumentTypeManager : IDocumentTypeManager
    {
        private readonly IActorRef _aggregateManager;

        public DocumentTypeManager([NotNull] IActorRef aggregateManager)
        {
            _aggregateManager = aggregateManager ?? throw new ArgumentNullException(nameof(aggregateManager));
        }

        public Task<IExecutionResult> ExecuteCommand(Command<DocumentTypeAggregate, DocumentTypeId> command, CancellationToken cancellation = default) =>
            _aggregateManager.Ask<IExecutionResult>(command);
    }
}