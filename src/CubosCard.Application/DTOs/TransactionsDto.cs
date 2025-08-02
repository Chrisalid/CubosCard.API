using CubosCard.Domain.Entities;

namespace CubosCard.Application.DTOs;

public class QueryTransactionsResponse
{
    public IEnumerable<TransactionResponse> Transactions { get; set; }
    public Pagination Pagination { get; set; }
}