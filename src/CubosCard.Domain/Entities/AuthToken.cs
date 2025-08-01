using System;

namespace CubosCard.Domain.Entities;

public class AuthToken : BaseEntity
{
    public Guid Id { get; set; }

    public string Token { get; set; }

    public Guid PersonId { get; set; }

    public DateTime ExpiresAt { get; set; }

    public virtual Person Person { get; set; }

    public static AuthToken Create(AuthTokenModel model)
    {
        AuthToken authToken = new() { Id = Guid.NewGuid() };

        authToken.SetToken(model.Token);
        authToken.SetPersonId(model.PersonId);
        authToken.SetExpiresAt(model.ExpiresAt);
        authToken.SetCreated(DateTime.Now);
        authToken.SetUpdated(DateTime.Now);

        return authToken;
    }

    public void SetToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be null or empty.", nameof(token));

        Token = token;
    }

    public void SetPersonId(Guid personId)
    {
        if (personId == Guid.Empty)
            throw new ArgumentException("PersonId cannot be empty.", nameof(personId));

        PersonId = personId;
    }

    public void SetExpiresAt(DateTime expiresAt)
    {
        ExpiresAt = expiresAt;
    }

    public record AuthTokenModel
    (
        string Token,
        Guid PersonId,
        DateTime ExpiresAt
    );
}
