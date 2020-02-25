using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akkatecture.Aggregates.ExecutionResults;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Commands;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using FluentValidation;
using MediatR;

namespace ElArch.WebApi.Controllers.DocumentTypes.Dto
{
    public sealed class ChangeDocumentTypeNameRequest : IRequest<IExecutionResult>
    {
        public string DocumentTypeId { get; set; }
        public string Name { get; set; }
    }

    public sealed class ChangeDocumentTypeNameRequestValidator : AbstractValidator<ChangeDocumentTypeNameRequest>
    {
        public ChangeDocumentTypeNameRequestValidator()
        {
            RuleFor(r => r.DocumentTypeId).NotEmpty().Custom((id, ctx) =>
            {
                var validationErrors = DocumentTypeId.Validate(id).ToArray();
                if (validationErrors.Length == 0) return;
                foreach (var validationError in validationErrors)
                {
                    ctx.AddFailure(validationError);
                }
            });
            RuleFor(r => r.Name).NotEmpty().MaximumLength(128);
        }
    }

    public sealed class ChangeDocumentTypeNameRequestHandler : IRequestHandler<ChangeDocumentTypeNameRequest, IExecutionResult>
    {
        private readonly IMediator _mediator;

        public ChangeDocumentTypeNameRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IExecutionResult> Handle(ChangeDocumentTypeNameRequest request, CancellationToken cancellationToken)
        {
            var id = DocumentTypeId.With(request.DocumentTypeId);
            var name = new DocumentTypeName(request.Name);
            var command = new ChangeDocumentTypeName(id, name);
            return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }
    }
}