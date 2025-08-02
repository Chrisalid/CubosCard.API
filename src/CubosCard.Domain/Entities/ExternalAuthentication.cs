using CubosCard.Domain.Enums;

namespace CubosCard.Domain.Entities;

public class ExternalAuthentication : BaseEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public CubosApiType Type { get; set; }

    public string? AuthCode { get; set; }

    public virtual ICollection<ExternalAuthenticationToken>  ExternalAuthenticationTokens { get; set; }

    public static ExternalAuthentication Create(ExternalAuthenticationModel model)
    {
        try
        {
            var externalAuthentication = new ExternalAuthentication() { Id = Guid.NewGuid() };

            externalAuthentication.SetEmail(model.Email);
            externalAuthentication.SetPassword(model.Password);
            externalAuthentication.SetType(model.Type);

            return externalAuthentication;
        }
        catch { throw; }
    }

    public void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be null or empty", nameof(email));

        Email = email;
    }

    public void SetPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        Password = password;
    }

    public void SetType(CubosApiType type)
    {
        if (!Enum.IsDefined(type))
            throw new ArgumentException("Type unnavaiable", nameof(type));

        Type = type;
    }

    public void SetAuthCode(string authCode)
    {
        if (string.IsNullOrWhiteSpace(authCode))
            throw new ArgumentException("AuthCode cannot be null or empty", nameof(authCode));

        AuthCode = authCode;
    }

    public record ExternalAuthenticationModel(
        string Email,
        string Password,
        string Url,
        CubosApiType Type
    );
}
