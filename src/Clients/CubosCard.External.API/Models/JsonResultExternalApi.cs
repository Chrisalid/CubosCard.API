namespace CubosCard.External.API.Models;

public class JsonResultExternalApi
{
    public bool? Success { get; set; }

    public dynamic? Data { get; set; }

    public string? Error { get; set; }
}
