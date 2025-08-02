using CubosCard.Application.DTOs;

namespace CubosCard.Application.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionResponse> CreateAsync(Guid accountId, TransactionRequest transactionRequest);

    Task<TransactionResponse> CreateInternalTransactionAsync(Guid accountId, InternalTransactionRequest internalTransactionRequest);
}
