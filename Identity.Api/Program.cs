using Identity.Api.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Identity.Api;

internal static class Program
{
    public static void Main(string[] args)
    {
        var jwtSettings = new JwtSettings(
            "http://localhost",
            "http://localhost",
            "VeryUnsecureWayToKeepTheKeyButItShouldBeLongEnoughToPassNow");

        const string key = "VeryUnsecureWayToKeepTheKeyButItShouldBeLongEnoughToPassNow";
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        Startup.ConfigureServices(builder.Services, jwtSettings, key);
        WebApplication app = builder.Build();
        BuilderApplication(app);
        app.Run();
    }

    private static void BuilderApplication(WebApplication app)
    {
        app.UseAuthentication().UseAuthorization();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(
                o =>
                {
                    o.SwaggerEndpoint("/swagger/v1/swagger.json", "JWT Login Api V1");
                    o.RoutePrefix = string.Empty; 
                    
                });
        }

        app.MapControllers();
    }
}