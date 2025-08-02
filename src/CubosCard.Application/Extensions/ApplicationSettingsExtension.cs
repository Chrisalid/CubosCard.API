using CubosCard.Application.Extensions.Options;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CubosCard.Application.Extensions;

public static class ApplicationSettingsExtension
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var resilienceOptions = new ResilienceOptions();
        configuration.GetSection("Resilience").Bind(resilienceOptions);

        services
            .AddScoped<IPersonService, PersonService>()
            .AddScoped<ICardService, CardService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IAuthTokenService, AuthTokenService>()
            .AddScoped<ITransactionService, TransactionService>();

        // services.ConfigureClientApi(configuration);

        return services;
    }
}
