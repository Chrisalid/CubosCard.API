using System.Threading.Tasks;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Application.Services;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.External.API;
using CubosCard.External.API.Interfaces.Services;
using CubosCard.Infrastructure.Data;
using CubosCard.IoC.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Refit;

namespace CubosCard.IoC.Extensions;

public static class ApplicationSettingsExtension
{
    // private readonly 
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        var resilienceOptions = new ResilienceOptions();
        configuration.GetSection("Resilience").Bind(resilienceOptions);

        services
            .AddScoped<ICardService, CardService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IAuthTokenService, AuthTokenService>()
            .AddScoped<ITransactionService, TransactionService>();

        services = services.ConfigureClientApi(configuration);

        services
            .AddScoped<IPersonService, PersonService>();

        return services;
    }

    private static IServiceCollection ConfigureClientApi(this IServiceCollection services, IConfiguration configuration)
    {
        var complianceApiUrl = configuration["ExternalApis:ComplianceApiUrl"];
        if (string.IsNullOrWhiteSpace(complianceApiUrl))
            throw new Exception("URL da API externa não foi encontrada na configuração.");

        services
            .AddRefitClient<ICubosComplianceApiRequest>()
            .AddHttpMessageHandler(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<LoggingHandler>>();
                return new LoggingHandler(logger);
            })
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(complianceApiUrl);
            });

        services
            .AddScoped<ICubosComplianceApiService, CubosComplianceApiService>();

        return services;
    }
}

public class LoggingHandler(ILogger<LoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Method} {RequestUri}", request.Method, request.RequestUri);

        if (request.Content != default)
        {
            logger.LogInformation("{Request}", await request.Content.ReadAsStringAsync(cancellationToken));
        }

        var response = await base.SendAsync(request, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        logger.LogInformation("{ResponseContent}", responseContent);

        return response;
    }
}
