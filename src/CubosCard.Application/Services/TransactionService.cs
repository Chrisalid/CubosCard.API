using System.Transactions;
using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using static CubosCard.Domain.Entities.Transaction;
using Transaction = CubosCard.Domain.Entities.Transaction;

namespace CubosCard.Application.Services;


public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountRepository _accountRepository;
    private readonly IConfiguration _configuration;

    public TransactionService(
        ITransactionRepository transactionRepository,
        IAccountRepository accountRepository,
        IConfiguration configuration)
    {
        _transactionRepository = transactionRepository;
        _accountRepository = accountRepository;
        _configuration = configuration;
    }

    public async Task<TransactionResponse> CreateAsync(Guid accountId, TransactionRequest transactionRequest)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId)
                ?? throw new ArgumentException("Account not Found!", nameof(accountId));

            if (transactionRequest.Value < 0 && account.Amount + transactionRequest.Value < 0)
                throw new ArgumentException("This account has no balance!", nameof(account));

            decimal newValueAccount = account.Amount + transactionRequest.Value;

            var transaction = Create(new TransactionModel(
                account.Id,
                transactionRequest.Value,
                transactionRequest.Description,
                transactionRequest.Value > 0 ? TransactionType.Credit
                                             : TransactionType.Debit
            ));

            await _transactionRepository.Create(transaction);

            account.Amount = newValueAccount;

            await _accountRepository.Update(account);

            return new TransactionResponse
            {
                Id = transaction.Id,
                Value = transaction.Value,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
        catch { throw; }
    }

    public async Task<TransactionResponse> CreateInternalTransactionAsync(Guid accountId, InternalTransactionRequest internalTransactionRequest)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId)
                ?? throw new ArgumentException("Account not Found!", nameof(accountId));

            var receiverAccount = await _accountRepository.GetById(internalTransactionRequest.ReceiverAccountId)
                ?? throw new ArgumentException("Receiver account not Found!", nameof(internalTransactionRequest.ReceiverAccountId));

            var value = internalTransactionRequest.Value < 0 ? -internalTransactionRequest.Value : internalTransactionRequest.Value;

            if (account.Amount - value < 0)
                throw new ArgumentException("This account has no balance!", nameof(account));

            var transaction = Create(new TransactionModel(
                account.Id,
                -value,
                internalTransactionRequest.Description,
                TransactionType.Debit
            ));

            var receiverTransaction = Create(new TransactionModel(
                receiverAccount.Id,
                value,
                internalTransactionRequest.Description,
                TransactionType.Credit
            ));

            await _transactionRepository.Create(transaction);
            await _transactionRepository.Create(receiverTransaction);

            account.Amount -= value;
            account.UpdatedAt = DateTime.Now;

            receiverAccount.Amount += value;
            receiverAccount.UpdatedAt = DateTime.Now;

            await _accountRepository.Update(account);
            await _accountRepository.Update(receiverAccount);

            return new TransactionResponse
            {
                Id = transaction.Id,
                Value = transaction.Value,
                Description = transaction.Description,
                CreatedAt = transaction.CreatedAt,
                UpdatedAt = transaction.UpdatedAt
            };
        }
        catch { throw; }
    }

    public async Task<QueryTransactionsResponse> GetByPagination(Guid accountId, int pageSize, int pageIndex, TransactionType? type)
    {
        try
        {
            var account = await _accountRepository.GetById(accountId)
                ?? throw new ArgumentException("Account not Found!", nameof(accountId));

            var transactions = await _transactionRepository.GetByPagination(account.Id, pageSize, pageIndex, type);

            return new QueryTransactionsResponse()
            {
                Transactions = transactions.Items.Select(
                transaction => new TransactionResponse
                {
                    Id = transaction.Id,
                    Value = transaction.Value,
                    Description = transaction.Description,
                    CreatedAt = transaction.CreatedAt,
                    UpdatedAt = transaction.UpdatedAt
                }),
                Pagination = new Pagination {
                    ItemsPerPage = transactions.PageSize,
                    CurrentPage = transactions.PageIndex,
                    TotalItems = transactions.Items.Count,
                    TotalPages = transactions.TotalPages
                }
            };
        }
        catch { throw; }
    }
}
