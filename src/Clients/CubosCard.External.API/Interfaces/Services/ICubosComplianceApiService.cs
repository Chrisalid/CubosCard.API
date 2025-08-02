using System;
using CubosCard.Domain.Entities;

namespace CubosCard.External.API.Interfaces.Services;

public interface ICubosComplianceApiService
{
    Task<ExternalAuthentication> AuthCode(ExternalAuthentication externalAuthentication);

    Task<ExternalAuthenticationToken> AuthToken(ExternalAuthentication externalAuthentication);

    Task<ExternalAuthenticationToken> AuthRefresh(ExternalAuthenticationToken externalAuthenticationToken);

    Task<bool> CpfValidate(ExternalAuthenticationToken externalAuthenticationToken, string socialNumber);

    Task<bool> CnpjValidate(ExternalAuthenticationToken externalAuthenticationToken, string taxNumber);
}
