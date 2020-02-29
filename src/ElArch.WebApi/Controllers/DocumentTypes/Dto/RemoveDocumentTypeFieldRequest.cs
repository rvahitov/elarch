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
    public sealed class RemoveDocumentTypeFieldRequest : IRequest<IExecutionResult>
    {
        public string DocumentTypeId { get; set; }
        public string FieldId { get; set; }
    }

    public sealed class RemoveDocumentTypeFieldRequestValidator : AbstractValidator<RemoveDocumentTypeFieldRequest>
    {
        public RemoveDocumentTypeFieldRequestValidator()
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
            RuleFor(r => r.FieldId).NotEmpty();
        }
    }

    public sealed class RemoveDocumentTypeFieldRequestHandler : IRequestHandler<RemoveDocumentTypeFieldRequest, IExecutionResult>
    {
        private readonly IMediator _mediator;

        public RemoveDocumentTypeFieldRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IExecutionResult> Handle(RemoveDocumentTypeFieldRequest request, CancellationToken cancellationToken)
        {
            var documentTypeId = DocumentTypeId.With(request.DocumentTypeId);
            var fieldId = new FieldId(request.FieldId);
            var command = new RemoveDocumentTypeField(documentTypeId, fieldId);
            return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }
    }
}