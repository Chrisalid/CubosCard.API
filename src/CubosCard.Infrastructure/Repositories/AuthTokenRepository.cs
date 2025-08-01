using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;

namespace CubosCard.Infrastructure.Repositories;

public class AuthTokenRepository(ApplicationDbContext context) : UnitOfWorkRepository(context), IAuthTokenRepository { }
