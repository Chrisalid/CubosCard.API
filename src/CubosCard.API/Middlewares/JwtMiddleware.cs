using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CubosCard.Application.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;

namespace CubosCard.API.Middlewares;

public class JwtMiddleware(RequestDelegate next, IConfiguration configuration)
{
    private readonly RequestDelegate _next = next;
    private readonly IConfiguration _configuration = configuration;

    public async Task InvokeAsync(HttpContext context, IAuthTokenService authTokenService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            await AttachUserToContextAsync(context, authTokenService, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContextAsync(HttpContext context, IAuthTokenService authTokenService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? throw new InvalidOperationException("JWT Secret Key not configured"));
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ClockSkew = TimeSpan.Zero
            };
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            var isValid = await authTokenService.ValidateTokenAsync(token);

            if (isValid)
            {
                var personIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");
                if (personIdClaim != null)
                {
                    context.Items["User"] = personIdClaim.Value;
                }
            }
        }
        catch
        {
            // Não faz nada, usuário não autenticado
        }
    }
}
