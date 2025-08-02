using System.Diagnostics.CodeAnalysis;
using CubosCard.API.Extensions;
using CubosCard.API.Middlewares;
using CubosCard.IoC.Extensions;
using CubosCard.Infrastructure.Data;
using CubosCard.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .ConfigureDbContext(builder.Configuration)
    .RegisterServices(builder.Configuration)
    .AddJwtConfiguration(builder.Configuration)
    .AddRepositories()
    .AddApiSettings()
    .AddApiTools()
    .ConfigureCompression();

builder.Services.AddProblemDetails();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.WebHost
    .ConfigureKestrel(options => { options.AddServerHeader = false; })
    .UseIIS();

var app = builder.Build();

app.UseApiTools(app.Configuration);

app.UseMiddleware<JwtMiddleware>();

app.UseApiSettings()
    .UseResponseCompression()
    .UseExceptionHandler()
    .UseRouting();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();

namespace CubosCard.API
{
    [ExcludeFromCodeCoverage]
    public partial class Program
    {
        protected Program() { }
    }
}
