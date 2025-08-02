using CubosCard.Application.DTOs;

namespace CubosCard.Application.Interfaces.Services;

public interface ICardService
{
    Task<CardResponse> CreateAsync(Guid accountId, CardRequest model);
    Task<QueryCardsResponse> GetCardListAsync(QueryCardRequest model);
    Task<List<CardResponse>> GetByAccountId(Guid accountId);
}
