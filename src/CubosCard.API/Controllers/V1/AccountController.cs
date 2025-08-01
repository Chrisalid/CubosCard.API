using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CubosCard.API.Controllers.V1;

public class AccountsController(
    IAccountService accountService,
    ICardService cardService
) : BaseController
{
    private readonly IAccountService _accountService = accountService;
    private readonly ICardService _cardService = cardService;

    [HttpPost("/")]
    public async Task<ActionResult<AccountResponse>> CreateAccount(AccountRequest jsonLoginRequest)
    {
        try
        {
            var accountResponse = await _accountService.CreateAsync(jsonLoginRequest);
            return accountResponse is not null
                ? Ok(accountResponse)
                : BadRequest(new { Message = "Account solicitation has failed!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpGet("/")]
    public async Task<ActionResult<IEnumerable<AccountResponse>>> GetAccounts(AccountRequest jsonLoginRequest)
    {
        return Ok();
    }

    [HttpPost("/{accountId:guid}/cards")]
    public async Task<ActionResult<CardResponse>> CreateCard([FromRoute] Guid accountId, [FromBody] CardRequest cardRequest)
    {
        return Ok();
    }

    [HttpGet("/{accountId:guid}/cards")]
    public async Task<ActionResult<TransactionResponse>> GetCards(Guid accountId, [FromBody] TransactionRequest cardRequest)
    {
        return Ok();
    }

    [HttpPost("/{accountId:guid}/transactions")]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction(Guid accountId, [FromBody] TransactionRequest transactionRequest)
    {
        return Ok();
    }

    [HttpPost("/{accountId:guid}/transactions/internal")]
    public async Task<ActionResult<TransactionResponse>> CreateInternalTransaction(Guid accountId, [FromBody] InternalTransactionRequest transactionRequest)
    {
        return Ok();
    }

    [HttpGet("/{accountId:guid}/transactions/internal")]
    public async Task<ActionResult<QueryCardsResponse>> GetCardList(
        Guid accountId,
        [FromQuery] int? itemsPerPage,
        [FromQuery] int? currentPage
    )
    {
        int pageSize = itemsPerPage ?? 10;
        int pageIndex = currentPage ?? 1;
        return Ok();
    }

    [HttpGet("/{accountId:guid}/balance")]
    public async Task<ActionResult<BalanceResponse>> GetCardList(Guid accountId)
    {
        return Ok();
    }
}
