using Akkatecture.Aggregates.ExecutionResults;
using MediatR;

namespace WebApplication.Controllers.DocumentTypeModel.Requests
{
    public sealed class CreateDocumentTypeRequest : IRequest<IExecutionResult>
    {
    }
}