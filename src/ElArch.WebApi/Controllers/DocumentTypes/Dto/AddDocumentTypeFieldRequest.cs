using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akkatecture.Aggregates.ExecutionResults;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Commands;
using FluentValidation;
using MediatR;

namespace ElArch.WebApi.Controllers.DocumentTypes.Dto
{
    public sealed class AddDocumentTypeFieldRequest : IRequest<IExecutionResult>
    {
        public string DocumentTypeId { get; set; }
        public FieldDto Field { get; set; }
    }

    public sealed class AddDocumentTypeFieldRequestValidator : AbstractValidator<AddDocumentTypeFieldRequest>
    {
        public AddDocumentTypeFieldRequestValidator()
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
            RuleFor(r => r.Field).Cascade(CascadeMode.StopOnFirstFailure).NotNull().SetValidator(new FieldDtoValidator());
        }
    }

    public sealed class AddDocumentTypeFieldRequestHandler : IRequestHandler<AddDocumentTypeFieldRequest, IExecutionResult>
    {
        private readonly IMediator _mediator;

        public AddDocumentTypeFieldRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IExecutionResult> Handle(AddDocumentTypeFieldRequest request, CancellationToken cancellationToken)
        {
            var documentTypeId = DocumentTypeId.With(request.DocumentTypeId);
            var field = request.Field.ToDomainModel();
            var command = new AddDocumentTypeField(documentTypeId, field);
            return await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
        }
    }
}