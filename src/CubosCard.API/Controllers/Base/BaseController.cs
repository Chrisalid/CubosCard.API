using System.Security.Claims;
using CubosCard.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace CubosCard.API.Controllers;

[ApiController]
[Route("[apiName]/v{version:apiVersion}/[controller]")]
public class BaseController : ControllerBase
{
    protected Guid GetCurrentPersonId()
    {
        var authTokenPersonId = HttpContext.Items["User"]?.ToString();

        if (!string.IsNullOrWhiteSpace(authTokenPersonId) && Guid.TryParse(authTokenPersonId, out var personId))
            return personId;

        var authTokenPersonIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!string.IsNullOrWhiteSpace(authTokenPersonIdClaim) && Guid.TryParse(authTokenPersonIdClaim, out var personIdClaim))
            return personIdClaim;

        throw new UnauthorizedAccessException("Usuário não autenticado ou token inválido");
    }

    public sealed class CustomValueRoutingConvention : IActionModelConvention
    {
        public void Apply(ActionModel action)
        {
            if (!action.RouteValues.TryGetValue(ApiSettingsExtension.projectNameSpace, out _))
                action.RouteValues.Add("apiName", ApiSettingsExtension.projectNameSpace);
        }
    }
}
