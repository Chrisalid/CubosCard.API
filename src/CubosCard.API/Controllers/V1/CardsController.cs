using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CubosCard.API.Controllers.V1;

[ApiController]
public class CardsController(ICardService cardService) : BaseController
{
    private readonly ICardService _cardService = cardService;

    [HttpGet]
    public async Task<ActionResult<QueryCardsResponse>> GetCardList([FromQuery] int? itemsPerPage, [FromQuery] int? currentPage)
    {
        try
        {

            int pageSize = itemsPerPage ?? 10;
            int pageIndex = currentPage ?? 1;

            var queryCardRequest = new QueryCardRequest
            {
                PersonId = GetCurrentPersonId(),
                CurrentPage = pageIndex,
                ItemsPerPage = pageSize
            };

            var queryCardResponse = await _cardService.GetCardListAsync(queryCardRequest);

            return queryCardResponse is not null
                ? Ok(queryCardResponse)
                : NotFound(new { Message = "Card List Not Found." });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}
