using System;
using CubosCard.Application.DTOs;

namespace CubosCard.Application.Interfaces.Services;

public interface IAccountService
{
    Task<AccountResponse> CreateAsync(AccountRequest model);
}
