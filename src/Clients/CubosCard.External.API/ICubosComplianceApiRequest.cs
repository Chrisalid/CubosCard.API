using CubosCard.External.API.Models;
using Refit;

namespace CubosCard.External.API;

public interface ICubosComplianceApiRequest
{
    [Post("/auth/code")]
    Task<JsonResultExternalApi> AuthCode(string email, string password);

    [Post("/auth/token")]
    Task<JsonResultExternalApi> AuthToken(string code);

    [Post("/auth/refresh")]
    Task<JsonResultExternalApi> AuthRefresh([Header("Authorization")] string token, string refreshToken);

    [Post("/cpf/validate")]
    Task<JsonResultExternalApi> CpfValidate([Header("Authorization")] string token, string socialSecurity);

    [Post("/cnpj/validate")]
    Task<JsonResultExternalApi> CnpjValidate([Header("Authorization")] string token, string taxNumber);
}
