#nullable enable
using System;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akkatecture.Aggregates.ExecutionResults;
using Akkatecture.Commands;
using MediatR;

namespace ElArch.Domain.Core.Commands
{
    public abstract class CommandRequestHandler<TCommand> : IRequestHandler<TCommand, IExecutionResult>
        where TCommand : ICommand, IRequest<IExecutionResult>
    {
        private readonly IActorRef _aggregateManager;

        protected CommandRequestHandler(IActorRef aggregateManager)
        {
            _aggregateManager = aggregateManager ?? throw new ArgumentNullException(nameof(aggregateManager));
        }

        public virtual async Task<IExecutionResult> Handle(TCommand command, CancellationToken cancellationToken)
        {
            return await _aggregateManager.Ask<IExecutionResult>(command, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}