using System;

namespace CubosCard.Application.DTOs;

public class LoginRequest
{
    public string Document { get; set; }
    public string Password { get; set; }
}

public class LoginResponse
{
    public string Token { get; set; }
}
