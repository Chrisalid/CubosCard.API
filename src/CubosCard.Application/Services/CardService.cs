using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Utility;
using Microsoft.Extensions.Configuration;

namespace CubosCard.Application.Services;

public class CardService : ICardService
{
    private readonly ICardRepository _cardRepository;

    private readonly IConfiguration _configuration;

    public CardService(ICardRepository cardRepository, IConfiguration configuration)
    {
        _cardRepository = cardRepository;
        _configuration = configuration;
    }

    public async Task<QueryCardsResponse> GetCardListAsync(QueryCardRequest model)
    {
        var cards = await _cardRepository.GetByPagination(model.PersonId, model.ItemsPerPage, model.CurrentPage);
        return new QueryCardsResponse
        {
            Cards = cards.Select(
            card => new CardResponse
            {
                Id = card.Id,
                Type = card.CardType,
                Number = Utils.CreateCardMask(card.Number),
                CVV = card.CVV,
                CreatedAt = card.CreatedAt,
                UpdatedAt = card.UpdatedAt
            })
        };
    }
}
