using System;
using System.Threading;
using System.Threading.Tasks;
using ElArch.Domain.Models.DocumentTypeModel;
using ElArch.Domain.Models.DocumentTypeModel.Commands;
using ElArch.Domain.Models.DocumentTypeModel.ValueObjects;
using FluentValidation;
using JetBrains.Annotations;
using MediatR;
using OneOf;
using OneOf.Types;

namespace ElArch.WebApi.Controllers.DocumentTypes.Dto
{
    public sealed class CreateDocumentTypeRequest : IRequest<OneOf<CreateDocumentTypeResponse, Error<string>>>
    {
        public string Name { get; set; }
    }

    public sealed class CreateDocumentTypeResponse
    {
        public CreateDocumentTypeResponse([NotNull] string documentTypeId)
        {
            if (string.IsNullOrEmpty(documentTypeId)) throw new ArgumentException("Value cannot be null or empty.", nameof(documentTypeId));
            DocumentTypeId = documentTypeId;
        }

        public string DocumentTypeId { get; }
    }

    public sealed class CreateDocumentTypeRequestValidator : AbstractValidator<CreateDocumentTypeRequest>
    {
        public CreateDocumentTypeRequestValidator()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(128);
        }
    }

    public sealed class CreateDocumentTypeRequestHandler : IRequestHandler<CreateDocumentTypeRequest, OneOf<CreateDocumentTypeResponse, Error<string>>>
    {
        private readonly IMediator _mediator;

        public CreateDocumentTypeRequestHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<OneOf<CreateDocumentTypeResponse, Error<string>>> Handle(CreateDocumentTypeRequest request, CancellationToken cancellationToken)
        {
            var id = DocumentTypeId.New;
            var name = new DocumentTypeName(request.Name);
            var command = new CreateDocumentType(id, name);
            var executionResult = await _mediator.Send(command, cancellationToken).ConfigureAwait(false);
            return executionResult.IsSuccess
                ? OneOf<CreateDocumentTypeResponse, Error<string>>.FromT0(new CreateDocumentTypeResponse(id.Value))
                : OneOf<CreateDocumentTypeResponse, Error<string>>.FromT1(new Error<string>(executionResult.ToString()));
        }
    }
}