using CubosCard.Domain.Entities;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.External.API.Interfaces.Services;
using CubosCard.External.API.Models;
using static CubosCard.Domain.Entities.ExternalAuthenticationToken;

namespace CubosCard.External.API;

public class CubosComplianceApiService(ICubosComplianceApiRequest api, IUnitOfWorkRepository repository) : ICubosComplianceApiService
{
    public async Task<ExternalAuthentication> AuthCode(ExternalAuthentication externalAuthentication)
    {
        try
        {
            var jsonAuthRequestToken = new JsonAuthCodeResquest
            {
                Email = externalAuthentication.Email,
                Password = externalAuthentication.Password
            };

            var jsonReturn = await api.AuthCode(jsonAuthRequestToken);

            string authCode = jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.GetProperty("authCode").GetString() : null;

            if (string.IsNullOrWhiteSpace(authCode))
                throw new ArgumentException(jsonReturn.Error, nameof(AuthCode));

            externalAuthentication.SetAuthCode(authCode);
            externalAuthentication.SetUpdated(DateTime.Now);

            await repository.Update(externalAuthentication);

            return externalAuthentication;
        }
        catch { throw; }
    }

    public async Task<ExternalAuthenticationToken> AuthToken(ExternalAuthentication externalAuthentication)
    {
        try
        {
            var jsonAuthRequestToken = new JsonAuthRequestToken { AuthCode = externalAuthentication.AuthCode };
            var jsonReturn = await api.AuthToken(jsonAuthRequestToken);

            var data = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? new JsonAuthResponseToken
                {
                    IdToken = jsonReturn.Data.GetProperty("idToken").GetString(),
                    AccessToken = jsonReturn.Data.GetProperty("accessToken").GetString(),
                    RefreshToken = jsonReturn.Data.GetProperty("refreshToken").GetString()
                }
                : null) ?? throw new ArgumentException(jsonReturn.Error, nameof(AuthToken));

            var externalAuthenticationToken = Create(new ExternalAuthenticationTokenModel(
                externalAuthentication.Id,
                data.IdToken,
                data.AccessToken,
                data.RefreshToken
            ));

            repository.Create(externalAuthenticationToken);

            return externalAuthenticationToken;
        }
        catch { throw; }
    }

    public async Task<ExternalAuthenticationToken> AuthRefresh(ExternalAuthenticationToken externalAuthenticationToken)
    {
        try
        {
            var jsonAuthRefreshRequest = new JsonAuthRefreshRequest { RefreshToken = externalAuthenticationToken.ExternalRefreshToken };
            var jsonReturn = await api.AuthRefresh("Bearer " + externalAuthenticationToken.ExternalAccessToken, jsonAuthRefreshRequest);

            string accessToken = jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.GetProperty("accessToken").GetString() : null;

            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException(jsonReturn.Error, nameof(AuthRefresh));

            externalAuthenticationToken.SetExternalAccessToken(accessToken);
            externalAuthenticationToken.SetUpdated(DateTime.Now);

            await repository.Update(externalAuthenticationToken);

            return externalAuthenticationToken;
        }
        catch { throw; }
    }

    public async Task<bool> CpfValidate(ExternalAuthenticationToken externalAuthenticationToken, string socialNumber)
    {
        try
        {
            var jsonDocumentValidate = new JsonDocumentValitate { Document = socialNumber };
            var jsonReturn = await api.CpfValidate("Bearer " + externalAuthenticationToken.ExternalAccessToken, jsonDocumentValidate);

            int? statusNum = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.Status : null)
                ?? throw new ArgumentException(jsonReturn.Error, nameof(CpfValidate));

            return statusNum.Value == 1;
        }
        catch { throw; }
    }

    public async Task<bool> CnpjValidate(ExternalAuthenticationToken externalAuthenticationToken, string taxNumber)
    {
        try
        {
            var jsonDocumentValidate = new JsonDocumentValitate { Document = taxNumber };
            var jsonReturn = await api.CnpjValidate("Bearer " + externalAuthenticationToken.ExternalAccessToken, jsonDocumentValidate);

            int? statusNum = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.Status : null)
                ?? throw new ArgumentException(jsonReturn.Error, nameof(CnpjValidate));

            return statusNum.Value == 1;
        }
        catch { throw; }
    }
}
