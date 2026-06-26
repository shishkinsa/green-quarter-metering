using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Exceptions;
using GQ.WebApi.UseCases.Handlers.Example.Mappings;
using GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById;

/// <summary>
/// Обработчик <see cref="GetExampleByIdQuery"/>.
/// </summary>
public sealed class GetExampleByIdQueryHandler(IExampleItemRepository repository)
    : IRequestHandler<GetExampleByIdQuery, GetExampleByIdResponse>
{
    public async Task<GetExampleByIdResponse> Handle(
        GetExampleByIdQuery query,
        CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(query.Id, cancellationToken);
        if (item is null)
        {
            throw new UseCaseNotFoundException($"Example item '{query.Id}' was not found.");
        }

        return new GetExampleByIdResponse(ExampleMappings.ToDto(item));
    }
}
