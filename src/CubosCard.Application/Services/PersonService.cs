using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CubosCard.Application.DTOs;
using CubosCard.Application.Interfaces.Services;
using CubosCard.Domain.Entities;
using CubosCard.Domain.Enums;
using CubosCard.Domain.Interfaces.Repositories;
using CubosCard.External.API.Interfaces.Services;
using CubosCard.Infrastructure.Utility;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using static CubosCard.Domain.Entities.AuthToken;
using static CubosCard.Domain.Entities.Person;

namespace CubosCard.Application.Services;

public class PersonService(
    IPersonRepository personRepository,
    IAuthTokenRepository authTokenRepository,
    IExternalAuthenticationRepository externalAuthenticationRepository,
    IExternalAuthenticationTokenRepository externalAuthenticationTokenRepository,
    ICubosComplianceApiService cubosComplianceApiService,
    IConfiguration configuration
) : IPersonService
{
    private readonly IPersonRepository _personRepository = personRepository;

    private readonly IAuthTokenRepository _authTokenRepository = authTokenRepository;

    private readonly IExternalAuthenticationRepository _externalAuthenticationRepository = externalAuthenticationRepository;

    private readonly IExternalAuthenticationTokenRepository _externalAuthenticationTokenRepository = externalAuthenticationTokenRepository;

    private readonly ICubosComplianceApiService _cubosComplianceApiService = cubosComplianceApiService;

    private readonly IConfiguration _configuration = configuration;

    public async Task<PersonResponse> CreateAsync(PersonModel model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Document))
                throw new ArgumentException("Document cannot be null or empty.", nameof(PersonModel));

            var document = Utils.NormalizeString(model.Document);

            if (!await VerifyCpfAndCpnjIsValid(document))
                throw new ArgumentException("Invalid document!", nameof(document));

            var existingPerson = await _personRepository.GetByDocument(document);
            if (existingPerson != null)
                throw new InvalidOperationException("A person with this document already exists.");

            var person = Create(new PersonModel(
                model.Name,
                document,
                BCrypt.Net.BCrypt.HashPassword(model.Password)
            ));

            await _personRepository.Create(person);

            return new PersonResponse
            {
                Id = person.Id,
                Name = person.Name,
                Document = person.Document,
                Password = model.Password
            };
        }
        catch { throw; }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest model)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(model.Document))
                throw new ArgumentException("Document cannot be null or empty.", nameof(PersonModel));

            var document = Utils.NormalizeString(model.Document);

            var person = await _personRepository.GetByDocument(document);
            if (person is null || !BCrypt.Net.BCrypt.Verify(model.Password, person.Password))
                return null;

            var tokenNotExpired = await _authTokenRepository.GetByPersonId(person.Id);
            if (tokenNotExpired is not null)
                return new LoginResponse { Token = tokenNotExpired.Token };

            var token = GenerateJwtToken(person, out DateTime expiresAt);

            var authToken = Create(new AuthTokenModel(
                token,
                person.Id,
                expiresAt
            ));

            await _authTokenRepository.Create(authToken);

            var loginResponse = new LoginResponse { Token = token };

            return loginResponse;
        }
        catch { throw; }
    }

    public string GenerateJwtToken(Person person, out DateTime expirationDate)
    {
        try
        {
            expirationDate = DateTime.Now.AddHours(24);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured"));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(ClaimTypes.NameIdentifier, person.Id.ToString()),
                        new Claim(ClaimTypes.Name, person.Name),
                        new Claim("Document", person.Document)
                    ]),
                Expires = expirationDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        catch { throw; }
    }

    public async Task<bool> VerifyCpfAndCpnjIsValid(string document)
    {
        try
        {
            var externalAuthentication = await _externalAuthenticationRepository.GetByType(CubosApiType.Compliance)
                ?? throw new ArgumentException("ExternalAuthentication not found!", nameof(VerifyCpfAndCpnjIsValid));

            var externalAuthenticationToken = await _externalAuthenticationTokenRepository.GetByExternalAuthenticationId(externalAuthentication.Id);
            if (externalAuthenticationToken is null)
            {
                externalAuthentication = await _cubosComplianceApiService.AuthCode(externalAuthentication);
                externalAuthenticationToken = await _cubosComplianceApiService.AuthToken(externalAuthentication);
            }
            else
                externalAuthenticationToken = await _cubosComplianceApiService.AuthRefresh(externalAuthenticationToken);

            return document.Length == 11
                ? await _cubosComplianceApiService.CpfValidate(externalAuthenticationToken, document)
                : await _cubosComplianceApiService.CnpjValidate(externalAuthenticationToken, document);
        }
        catch { throw; }
    }
}
