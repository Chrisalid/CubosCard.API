using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Utility;
using Microsoft.Extensions.Configuration;
using static CubosCard.Domain.Entities.Card;

namespace CubosCard.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    private readonly IAccountRepository _accountRepository;

    private readonly IConfiguration _configuration;

    public CardService(ICardRepository cardRepository, IAccountRepository accountRepository, IConfiguration configuration)
    {
        _cardRepository = cardRepository;
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public async Task<CardResponse> CreateAsync(Guid accountId, CardRequest model)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId) ??
                throw new ArgumentException("Account not found", nameof(accountId));

            var existsCard = model.Type == CardType.Physical
                ? await _cardRepository.GetByAccountAndType(accountId, CardType.Physical)
                : null;

            if (existsCard is not null)
                throw new ArgumentException("There is already a physical card for this account.", nameof(accountId));

            var cardNumber = Utils.NormalizeString(model.Number.Trim());

            var card = Create(new CardModel(
                account.Id,
                cardNumber,
                model.CVV,
                model.Type
            ));

            await _cardRepository.Create(card);

            return new CardResponse
            {
                Id = card.Id,
                Type = card.CardType.ToString().ToLower(),
                Number = Utils.GetLastFourDigits(card.Number),
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };
        }
        catch { throw; }
    }

    public async Task<QueryCardsResponse> GetCardListAsync(QueryCardRequest model)
    {
        try
        {
            var cards = await _cardRepository.GetByPagination(model.PersonId, model.ItemsPerPage, model.CurrentPage);

            return new QueryCardsResponse()
            {
                Cards = cards.Items.Select(
                card => new CardResponse
                {
                    Id = card.Id,
                    Type = card.CardType.ToString().ToLower(),
                    Number = Utils.CreateCardMask(card.Number),
                    CVV = card.CVV,
                    CreatedAt = card.CreatedAt,
                    UpdatedAt = card.UpdatedAt
                }),
                Pagination = new Pagination {
                    ItemsPerPage = cards.PageSize,
                    CurrentPage = cards.PageIndex,
                    TotalItems = cards.Items.Count,
                    TotalPages = cards.TotalPages
                }
            };
        }
        catch { throw; }
    }

    public async Task<List<CardResponse>> GetByAccountId(Guid accountId)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId)
                ?? throw new ArgumentException("Account not found", nameof(accountId));

            var cards = await _cardRepository.GetByAccountId(account.Id);

            return [.. cards.Select(
                card => new CardResponse
                {
                    Id = card.Id,
                    Type = card.CardType.ToString().ToLower(),
                    Number = Utils.CreateCardMask(card.Number),
                    CVV = card.CVV,
                    CreatedAt = card.CreatedAt,
                    UpdatedAt = card.UpdatedAt
                })];
        }
        catch { throw; }
    }
}
