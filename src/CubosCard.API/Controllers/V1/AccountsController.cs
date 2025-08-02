using CubosCard.API.Attributes;
using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace CubosCard.API.Controllers.V1;

[ApiController]
public class AccountsController(
    IAccountService accountService,
    ICardService cardService,
    ITransactionService transactionService
) : BaseController
{
    private readonly IAccountService _accountService = accountService;
    private readonly ICardService _cardService = cardService;
    private readonly ITransactionService _transactionService = transactionService;

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<AccountResponse>> CreateAccount(AccountRequest jsonLoginRequest)
    {
        try
        {
            var personId = GetCurrentPersonId();
            var accountResponse = await _accountService.CreateAsync(personId, jsonLoginRequest);
            return accountResponse is not null
                ? Ok(accountResponse)
                : BadRequest(new { Message = "Account solicitation has failed!" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<AccountResponse>>> GetAccounts()
    {
        try
        {
            var personId = GetCurrentPersonId();

            var accountResponse = await _accountService.GetByPersonIdAsync(personId);

            return accountResponse is not null && accountResponse.Count != 0
                ? Ok(accountResponse)
                : NotFound(new { Message = "No accounts are found!" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpPost("{accountId:guid}/cards")]
    public async Task<ActionResult<CardResponse>> CreateCard([FromRoute] Guid accountId, [FromBody] CardRequest cardRequest)
    {
        try
        {
            var cardResponse = await _cardService.CreateAsync(accountId, cardRequest);

            return cardResponse is not null
                ? Ok(cardResponse)
                : BadRequest(new { Message = "It was not possible to create a card for this account" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpGet("{accountId:guid}/cards")]
    public async Task<ActionResult<List<CardResponse>>> GetCards(Guid accountId)
    {
        try
        {
            var cardResponseList = await _cardService.GetByAccountId(accountId);

            return cardResponseList is not null && cardResponseList.Count != 0
                ? Ok(cardResponseList)
                : BadRequest("The transaction could not be created!");
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpPost("{accountId:guid}/transactions")]
    public async Task<ActionResult<TransactionResponse>> CreateTransaction(Guid accountId, [FromBody] TransactionRequest transactionRequest)
    {
        try
        {
            var transactionResponse = await _transactionService.CreateAsync(accountId, transactionRequest);

            return transactionResponse is not null
                ? Ok(transactionResponse)
                : BadRequest("The transaction could not be created!");
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpPost("{accountId:guid}/transactions/internal")]
    public async Task<ActionResult<TransactionResponse>> CreateInternalTransaction(Guid accountId, [FromBody] InternalTransactionRequest transactionRequest)
    {
        try
        {
            var transactionResponse = await _transactionService.CreateInternalTransactionAsync(accountId, transactionRequest);

            return transactionResponse is not null
                ? Ok(transactionResponse)
                : BadRequest(new { Message = "The internal transaction could not be created!" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpGet("{accountId:guid}/transactions")]
    public async Task<ActionResult<QueryCardsResponse>> GetTransactionList(
        Guid accountId,
        [FromQuery] int? itemsPerPage,
        [FromQuery] int? currentPage,
        [FromQuery] string? typeTransaction
    )
    {
        try
        {
            int pageSize = itemsPerPage ?? 10;
            int pageIndex = currentPage ?? 1;
            TransactionType? type = typeTransaction is not null ? typeTransaction.ToLower() switch
            {
                "credit" => TransactionType.Credit,
                "debit" => TransactionType.Debit,
                _ => null
            } : null;

            var transactionsPaginated = await _transactionService.GetByPagination(accountId, pageSize, pageIndex, type);

            return transactionsPaginated is not null && transactionsPaginated.Transactions.Count() != 0
                ? Ok(transactionsPaginated)
                : NotFound(new { Message = "We were unable to find cards with these specifications!" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }

    [Authorize]
    [HttpGet("{accountId:guid}/balance")]
    public async Task<ActionResult<BalanceResponse>> GetAccountBalance(Guid accountId)
    {
        try
        {
            var balanceResponse = await _accountService.GetBalanceResponseAsync(accountId);

            return balanceResponse is not null
                ? Ok(balanceResponse)
                : BadRequest(new { Message = "Não foi possível obter o valor da conta!" });
        }
        catch (Exception ex) { return BadRequest(new { ex.Message }); }
    }
}
