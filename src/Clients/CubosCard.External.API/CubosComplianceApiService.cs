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
            var jsonReturn = await api.AuthCode(externalAuthentication.Email, externalAuthentication.Password);

            string authCode = jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.AuthCode : null;

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
            var jsonReturn = await api.AuthToken(externalAuthentication.AuthCode);

            var data = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? new JsonAuthToken
                {
                    IdToken = jsonReturn.Data.idToken,
                    AccessToken = jsonReturn.Data.accessToken,
                    RefreshToken = jsonReturn.Data.refreshToken
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
            var jsonReturn = await api.AuthRefresh(externalAuthenticationToken.ExternalAccessToken, externalAuthenticationToken.ExternalRefreshToken);

            string accessToken = jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.AccessToken : null;

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
            var jsonReturn = await api.CpfValidate(externalAuthenticationToken.ExternalAccessToken, socialNumber);

            bool? status = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.Status : null) ?? throw new ArgumentException(jsonReturn.Error, nameof(CpfValidate));

            return status.Value;
        }
        catch { throw; }
    }

    public async Task<bool> CnpjValidate(ExternalAuthenticationToken externalAuthenticationToken, string taxNumber)
    {
        try
        {
            var jsonReturn = await api.CnpjValidate(externalAuthenticationToken.ExternalAccessToken, taxNumber);

            bool? status = (jsonReturn.Success.HasValue && jsonReturn.Success.Value
                ? jsonReturn.Data.Status : null)
                ?? throw new ArgumentException(jsonReturn.Error, nameof(CnpjValidate));

            return status.Value;
        }
        catch { throw; }
    }
}
