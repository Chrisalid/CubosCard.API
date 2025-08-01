using System;
using CubosCard.Domain.Entities;

namespace CubosCard.Domain.Interfaces.Repositories;

public interface IPersonRepository : IUnitOfWorkRepository
{
    Task<Person> GetById(Guid personId);
    Task<Person> GetByDocument(string document);
}
