using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.CreateExample.Responses;
using GQ.WebApi.UseCases.Handlers.Example.Commands.DeleteExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample;
using GQ.WebApi.UseCases.Handlers.Example.Commands.UpdateExample.Responses;
using GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById;
using GQ.WebApi.UseCases.Handlers.Example.Queries.GetExampleById.Responses;
using GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples;
using GQ.WebApi.UseCases.Handlers.Example.Queries.ListExamples.Responses;
using GQ.WebApi.WebApp.Authentication;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// REST API для эталонной сущности ExampleItem.
/// </summary>
[ApiController]
[Route("api/v1/examples")]
public sealed class ExamplesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Возвращает список примеров.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ListExamplesResponse), StatusCodes.Status200OK)]
    public Task<ListExamplesResponse> List(CancellationToken cancellationToken) =>
        mediator.Send(new ListExamplesQuery(), cancellationToken);

    /// <summary>
    /// Возвращает пример по идентификатору.
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(GetExampleByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<GetExampleByIdResponse> GetById(Guid id, CancellationToken cancellationToken) =>
        mediator.Send(new GetExampleByIdQuery(id), cancellationToken);

    /// <summary>
    /// Создаёт новый пример.
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthPolicies.AuthenticatedWhenEnabled)]
    [ProducesResponseType(typeof(CreateExampleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<CreateExampleResponse>> Create(
        [FromBody] CreateExampleRequest request,
        CancellationToken cancellationToken)
    {
        var response = await mediator.Send(new CreateExampleCommand(request.Name), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Item.Id }, response);
    }

    /// <summary>
    /// Обновляет пример по идентификатору.
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UpdateExampleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<UpdateExampleResponse> Update(
        Guid id,
        [FromBody] UpdateExampleRequest request,
        CancellationToken cancellationToken) =>
        mediator.Send(new UpdateExampleCommand(id, request.Name), cancellationToken);

    /// <summary>
    /// Удаляет пример по идентификатору.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteExampleCommand(id), cancellationToken);
        return NoContent();
    }
}

/// <summary>
/// Тело запроса создания примера.
/// </summary>
public sealed record CreateExampleRequest(string Name);

/// <summary>
/// Тело запроса обновления примера.
/// </summary>
public sealed record UpdateExampleRequest(string Name);
