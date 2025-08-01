using CubosCard.Application.DTOs;
using CubosCard.Domain.Entities;
using static CubosCard.Domain.Entities.Person;

namespace CubosCard.Application.Interfaces.Services;

public interface IPersonService
{
    Task<PersonResponse> CreateAsync(PersonModel model);

    Task<LoginResponse?> LoginAsync(LoginRequest loginRequest);
}