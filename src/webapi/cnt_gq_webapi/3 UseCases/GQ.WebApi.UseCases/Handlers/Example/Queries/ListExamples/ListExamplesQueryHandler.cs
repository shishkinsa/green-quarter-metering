using GQ.WebApi.Infrastructure.Interfaces.Repositories;
using GQ.WebApi.UseCases.Handlers.Example.Mappings;
using GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples.Responses;
using MediatR;

namespace GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples;

/// <summary>
/// Обработчик <see cref="ListExamplesQuery"/>.
/// </summary>
public sealed class ListExamplesQueryHandler(IExampleItemRepository repository)
    : IRequestHandler<ListExamplesQuery, ListExamplesResponse>
{
    public async Task<ListExamplesResponse> Handle(
        ListExamplesQuery query,
        CancellationToken cancellationToken)
    {
        var items = await repository.ListAsync(cancellationToken);
        var dtos = items.Select(ExampleMappings.ToDto).ToList();
        return new ListExamplesResponse(dtos);
    }
}
