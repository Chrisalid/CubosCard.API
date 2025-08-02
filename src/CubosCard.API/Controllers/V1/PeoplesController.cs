using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using static CubosCard.Domain.Entities.Person;

namespace CubosCard.API.Controllers;

[ApiController]
public class PeoplesController(IPersonService personService) : BaseController
{
    private readonly IPersonService _personService = personService;

    [HttpPost]
    public async Task<ActionResult<PersonResponse>> CreatePerson(PersonRequest jsonCreatePersonRequest)
    {
        try
        {
            var personModel = new PersonModel(
                jsonCreatePersonRequest.Name,
                jsonCreatePersonRequest.Document,
                jsonCreatePersonRequest.Password
            );

            var personResponse = await _personService.CreateAsync(personModel);

            return personResponse is not null
                ? Ok(personResponse)
                : BadRequest(new { Message = "Unable to create person!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
