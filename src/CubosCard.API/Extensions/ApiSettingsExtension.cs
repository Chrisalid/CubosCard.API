using System.Text;
using CubosCard.API.Middlewares;
using CubosCard.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using static CubosCard.API.Controllers.BaseController;

namespace CubosCard.API.Extensions;

public static class ApiSettingsExtension
{
    public const string projectNameSpace = "cuboscard-api";

    public static IServiceCollection AddApiTools(this IServiceCollection services) =>
        services.AddEndpointsApiExplorer()
            .AddApiVersioning(v =>
            {
                v.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                v.ReportApiVersions = true;
                v.AssumeDefaultVersionWhenUnspecified = true;
            }).AddVersionedApiExplorer(p =>
            {
                p.GroupNameFormat = "'v'VVV";
                p.SubstituteApiVersionInUrl = true;
            }).AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CubosCard API", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

    public static IServiceCollection AddJwtConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var secretKey = jwtSettings["SecretKey"];

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
            };
        });

        return services;
    }

    public static IApplicationBuilder UseApiSettings(this IApplicationBuilder app) =>
        app.UseLoggingMiddleware()
            .UseAuthentication()
            .UseRouting()
            .UseAuthorization()
            .UseEndpoints(endPoints => { endPoints.MapControllers(); });

    public static IServiceCollection AddApiSettings(this IServiceCollection services)
    {
        services
            .AddRouting(options => options.LowercaseUrls = true)
            .AddControllers(mvcOptions => mvcOptions.Conventions.Add(new CustomValueRoutingConvention()))
            .AddJsonOptions(options => { options.JsonSerializerOptions.AddJsonSerializerOptions(); });

        return services;
    }

    public static IApplicationBuilder UseApiTools(this IApplicationBuilder app, IConfiguration configuration)
    {
        var apiVersionDescriptionProvider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();
        var path = configuration["SwaggerApiSuffix"] ?? string.Empty;

        return app
            .UseSwagger(opt => opt.PreSerializeFilters.Add((doc, req) =>
                doc.Servers = [new() { Url = $"{req.Scheme}://{req.Headers.Host}/{path}" }]
            ))
            .UseSwaggerUI(c =>
            {
                apiVersionDescriptionProvider.ApiVersionDescriptions
                    .Select(desc => desc.GroupName)
                    .ToList()
                    .ForEach(groupName =>
                    {
                        c.SwaggerEndpoint($"{groupName}/swagger.json", groupName.ToUpperInvariant());
                    });

                c.DocExpansion(DocExpansion.List);
            })
            .UseHttpsRedirection();
    }

    public static IServiceCollection ConfigureCompression(this IServiceCollection services)
    {
        services.Configure<GzipCompressionProviderOptions>(options => options.Level = System.IO.Compression.CompressionLevel.Optimal);
        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.MimeTypes = ["application/json"];
            options.Providers.Add<GzipCompressionProvider>();
        });

        return services;
    }

    private static IApplicationBuilder UseLoggingMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<LoggingMiddleware>();
    }
}
