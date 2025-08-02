using CubosCard.External.API.Models;
using Refit;

namespace CubosCard.External.API;

public interface ICubosComplianceApiRequest
{
    [Post("/auth/code")]
    Task<JsonResultExternalApi> AuthCode([Body] JsonAuthCodeResquest jsonAuthCodeResquest);

    [Post("/auth/token")]
    Task<JsonResultExternalApi> AuthToken([Body] JsonAuthRequestToken jsonAuthRequestToken);

    [Post("/auth/refresh")]
    Task<JsonResultExternalApi> AuthRefresh([Header("Authorization")] string token, [Body] JsonAuthRefreshRequest jsonAuthRefreshRequest);

    [Post("/cpf/validate")]
    Task<JsonResultExternalValidationApi> CpfValidate([Header("Authorization")] string token, [Body] JsonDocumentValitate jsonDocumentValitate);

    [Post("/cnpj/validate")]
    Task<JsonResultExternalValidationApi> CnpjValidate([Header("Authorization")] string token, [Body] JsonDocumentValitate jsonDocumentValitate);
}
