using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Utility;
using Microsoft.Extensions.Configuration;
using static CubosCard.Domain.Entities.Account;

namespace CubosCard.Application.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepository;

    private readonly IPersonRepository _personRepository;

    private readonly IConfiguration _configuration;

    public AccountService(IAccountRepository accountRepository,
    IPersonRepository personRepository,
    IConfiguration configuration)
    {
        _accountRepository = accountRepository;
        _personRepository = personRepository;
        _configuration = configuration;
    }

    public async Task<AccountResponse> CreateAsync(AccountRequest model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Branch) && model.Branch.Length != 3)
                throw new ArgumentException("Branch cannot be null, empty or have more or less than 3 characters.", nameof(AccountRequest));

            if (!Utils.IsValidAccountNumber(model.Account))
                throw new ArgumentException("Account number does not match mask XXXXXXX-X.", nameof(AccountRequest));

            var person = await _personRepository.GetById(model.PersonId) ??
                throw new ArgumentException("Person not found", nameof(Person));

            var account = Create(new AccountModel(
                model.Branch,
                model.Account,
                person.Id,
                0.0m
            ));

            await _accountRepository.Create(account);

            return new AccountResponse
            {
                Id = person.Id,
                Branch = account.Branch,
                Account = account.AccountNumber,
                CreatedAt = account.CreatedAt,
                UpdatedAt = account.UpdatedAt
            };
        }
        catch
        {
            throw;
        }
    }
}
