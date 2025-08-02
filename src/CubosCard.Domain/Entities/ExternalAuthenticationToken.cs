namespace CubosCard.Domain.Entities;

public class ExternalAuthenticationToken : BaseEntity
{
    public Guid Id { get; set; }

    public Guid ExternalAuthenticationId { get; set; }

    public string ExternalTokenId { get; set; }

    public string ExternalAccessToken { get; set; }

    public string ExternalRefreshToken { get; set; }

    public virtual ExternalAuthentication ExternalAuthentication { get; set; }

    public static ExternalAuthenticationToken Create(ExternalAuthenticationTokenModel model)
    {
        try
        {
            var externalAuthenticationToken = new ExternalAuthenticationToken() { Id = Guid.NewGuid() };

            externalAuthenticationToken.SetExternalAuthenticationId(model.ExternalAuthenticationId);
            externalAuthenticationToken.SetExternalTokenId(model.ExternalTokenId);
            externalAuthenticationToken.SetExternalAccessToken(model.ExternalAccessToken);
            externalAuthenticationToken.SetExternalRefreshToken(model.ExternalRefreshToken);

            externalAuthenticationToken.SetCreated(DateTime.Now);
            externalAuthenticationToken.SetUpdated(DateTime.Now);

            return externalAuthenticationToken;
        }
        catch { throw; }
    }

    public void SetExternalAuthenticationId(Guid externalAuthenticationId)
    {
        if (externalAuthenticationId == Guid.Empty)
            throw new ArgumentException("ExternalAuthenticationId cannot be null", nameof(externalAuthenticationId));

        ExternalAuthenticationId = externalAuthenticationId;
    }

    public void SetExternalTokenId(string externalTokenId)
    {
        if (string.IsNullOrWhiteSpace(externalTokenId))
            throw new ArgumentException("ExternalTokenId cannot be null", nameof(externalTokenId));

        ExternalTokenId = externalTokenId;
    }

    public void SetExternalAccessToken(string externalAccessToken)
    {
        if (string.IsNullOrWhiteSpace(externalAccessToken))
            throw new ArgumentException("ExternalAccessToken cannot be null", nameof(externalAccessToken));

        ExternalAccessToken = externalAccessToken;
    }

    public void SetExternalRefreshToken(string externalRefreshToken)
    {
        if (string.IsNullOrWhiteSpace(externalRefreshToken))
            throw new ArgumentException("ExternalRefreshToken cannot be null", nameof(externalRefreshToken));

        ExternalRefreshToken = externalRefreshToken;
    }

    public record ExternalAuthenticationTokenModel(
        Guid ExternalAuthenticationId,
        string ExternalTokenId,
        string ExternalAccessToken,
        string ExternalRefreshToken
    );
}
