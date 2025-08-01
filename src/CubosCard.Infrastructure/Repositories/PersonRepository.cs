using System;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CubosCard.Infrastructure.Repositories;

public class PersonRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IPersonRepository
{
    public async Task<Person> GetById(Guid personId)
    {
        return await _dbContext.Set<Person>().Where(_ => _.Id == personId).FirstOrDefaultAsync();
    }

    public async Task<Person> GetByDocument(string document)
    {
        return await _dbContext.Set<Person>().Where(_ => _.Document == document).FirstOrDefaultAsync();
    }
}
