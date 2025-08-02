using CubosCard.Application.DTOs;
using CubosCard.Domain.Enums;

namespace CubosCard.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(Guid accountId, TransactionRequest transactionRequest);

    Task<TransactionResponse> CreateInternalTransactionAsync(Guid accountId, InternalTransactionRequest internalTransactionRequest);

    Task<QueryTransactionsResponse> GetByPagination(Guid accountId, int pageSize, int pageIndex, TransactionType? type);
}
