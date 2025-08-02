using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.Infrastructure.Data;
using CubosCard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CubosCard.Infrastructure.Extensions;

public static class InfrastructureSettingsExtension
{
    public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<ApplicationDbContext>(options =>
             options.UseNpgsql(configuration.GetConnectionString("ApplicationDbContext"))
             .UseSnakeCaseNamingConvention()
             .EnableSensitiveDataLogging(bool.Parse(configuration["DbContextOptions:SensitiveDataLoggingEnabled"] ?? "false"))
             .EnableDetailedErrors(bool.Parse(configuration["DbContextOptions:DetailedErrorsEnabled"] ?? "false")));

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services
        .AddScoped<IUnitOfWorkRepository, UnitOfWorkRepository>()
        .AddScoped<IPersonRepository, PersonRepository>()
        .AddScoped<IAccountRepository, AccountRepository>()
        .AddScoped<ICardRepository, CardRepository>()
        .AddScoped<ITransactionRepository, TransactionRepository>()
        .AddScoped<IAuthTokenRepository, AuthTokenRepository>()
        .AddScoped<IExternalAuthenticationRepository, ExternalAuthenticationRepository>()
        .AddScoped<IExternalAuthenticationTokenRepository, ExternalAuthenticationTokenRepository>();
    }
}
