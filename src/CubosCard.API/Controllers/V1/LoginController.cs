using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CubosCard.API.Controllers.V1;

[ApiController]
public class LoginController(IPersonService personService) : BaseController
{
    private readonly IPersonService _personService = personService;

    [HttpPost]
    public async Task<ActionResult<LoginResponse>> LoginPerson(LoginRequest jsonLoginRequest)
    {
        try
        {
            var loginResponse = await _personService.LoginAsync(jsonLoginRequest);

            return loginResponse is not null && !string.IsNullOrWhiteSpace(loginResponse.Token)
                ? Ok(new { loginResponse.Token })
                : NotFound(new { Message = "Invalid login credentials." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
