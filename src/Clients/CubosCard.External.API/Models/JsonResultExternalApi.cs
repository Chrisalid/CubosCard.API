namespace CubosCard.External.API.Models;

public class JsonResultExternalApi
{
    public bool? Success { get; set; }

    public dynamic? Data { get; set; }

    public string? Error { get; set; }
}

public class JsonResultExternalValidationApi
{
    public bool? Success { get; set; }

    public JsonResultExternalValidationData? Data { get; set; }

    public string? Error { get; set; }
}

public class JsonResultExternalValidationData
{
    public string Document { get; set; }

    public int? Status { get; set; }

    public string Reason { get; set; }
}
