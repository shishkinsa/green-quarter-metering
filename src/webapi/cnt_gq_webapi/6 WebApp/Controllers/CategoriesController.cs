using Microsoft.AspNetCore.Mvc;
using MediatR;
using GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories;
using GQ.WebApi.UseCases.Handlers.Category.Queries.ListCategories.Responses;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// REST API справочника категорий (read-only).
/// </summary>
[ApiController]
[Route("api/v1/categories")]
public sealed class CategoriesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Возвращает список категорий.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ListCategoriesResponse), StatusCodes.Status200OK)]
    public Task<ListCategoriesResponse> List(CancellationToken cancellationToken) =>
        mediator.Send(new ListCategoriesQuery(), cancellationToken);
}
