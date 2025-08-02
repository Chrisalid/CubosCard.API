using CubosCard.External.API.Models;
using Refit;

namespace CubosCard.External.API;

public interface ICubosComplianceApiRequest
{
    Task<JsonResultExternalApi> AuthCode(string email, string password);

    Task<JsonResultExternalApi> AuthToken(string code);
    
    Task<JsonResultExternalApi> AuthRefresh([Header("Authorization")] string token, string refreshToken);

    Task<JsonResultExternalApi> CpfValidate([Header("Authorization")] string token, string socialSecurity);

    Task<JsonResultExternalApi> CnpjValidate([Header("Authorization")] string token, string taxNumber);
}
