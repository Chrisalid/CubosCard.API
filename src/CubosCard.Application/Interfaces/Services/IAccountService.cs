using System;
using CubosCard.Application.DTOs;

namespace CubosCard.Application.Interfaces.Services;

public interface IAccountService
{
    Task<List<AccountResponse>> GetByPersonIdAsync(Guid personId);
    Task<AccountResponse> CreateAsync(AccountRequest model);

    Task<BalanceResponse> GetBalanceResponseAsync(Guid accountId);
}
