namespace CubosCard.External.API.Models;

public class JsonAuthRequestToken
{
    public string AuthCode { get; set; }
}

public class JsonAuthResponseToken
{
    public string IdToken { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
