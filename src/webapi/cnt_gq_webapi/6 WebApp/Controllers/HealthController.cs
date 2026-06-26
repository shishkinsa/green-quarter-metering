using Microsoft.AspNetCore.Mvc;

namespace GQ.WebApi.WebApp.Controllers;

/// <summary>
/// Системные эндпоинты API.
/// </summary>
[ApiController]
[Route("api/v1/health")]
public sealed class HealthController: ControllerBase
{
    /// <summary>
    /// Возвращает статус приложения (дублирует /health для REST-контракта OpenAPI).
    /// </summary>
    [HttpGet(Name = "getHealth")]
    [ProducesResponseType(typeof(HealthResponse), StatusCodes.Status200OK)]
    public ActionResult<HealthResponse> Get()
    {
        return Ok(new HealthResponse("ok", "GQ.WebApi.WebApp"));
    }
}

/// <summary>
/// Ответ проверки готовности сервиса.
/// </summary>
/// <param name="Status">Статус сервиса.</param>
/// <param name="Service">Имя сервиса.</param>
public sealed record HealthResponse(string Status, string Service);
