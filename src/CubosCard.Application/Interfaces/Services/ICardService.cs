using CubosCard.Application.DTOs;

namespace CubosCard.Application.Interfaces.Services;

public interface ICardService
{
    Task<QueryCardsResponse> GetCardListAsync(QueryCardRequest model);
}
