using CubosCard.Domain.Enums;

namespace CubosCard.Application.DTOs;

public class QueryCardsResponse
{
    public IEnumerable<CardResponse> Cards { get; set; }

    public Pagination Pagination { get; set; }
}

public class QueryCardRequest
{
    public Guid PersonId { get; set; }

    public int ItemsPerPage { get; set; }

    public int CurrentPage { get; set; }

    public CardType CardType { get; set; }
}
