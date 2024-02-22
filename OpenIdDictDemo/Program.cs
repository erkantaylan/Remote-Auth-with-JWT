using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace OpenIdDictDemo;

//https://medium.com/@sergeygoodgood/openid-connect-and-oauth2-0-server-in-aspnetcore-using-openiddict-c463c6ebc082
internal static class Program
{
    public static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddOpenIddict()
               .AddCore(coreBuilder => { coreBuilder.UseEntityFrameworkCore().UseDbContext<DbContext>(); })
               .AddServer(
                   serverBuilder =>
                   {
                       serverBuilder.AllowRefreshTokenFlow();
                       serverBuilder.AllowClientCredentialsFlow();
                       serverBuilder.SetTokenEndpointUris("token");
                       serverBuilder.SetRevocationEndpointUris("token/revoke");
                       serverBuilder.SetIntrospectionEndpointUris("token/introspect");

                       serverBuilder.AddDevelopmentEncryptionCertificate();
                       serverBuilder.AddDevelopmentSigningCertificate();

                       serverBuilder.DisableAccessTokenEncryption();

                       serverBuilder.UseAspNetCore().EnableTokenEndpointPassthrough();
                   });

        builder.Services.AddDbContext<DbContext>(
            optionsBuilder =>
            {
                optionsBuilder.UseInMemoryDatabase(nameof(DbContext));
                optionsBuilder.UseOpenIddict();
            });

        builder.Services.AddHostedService<ClientSeeder>();

        WebApplication app = builder.Build();

        app.UseAuthentication();

        app.MapGet("/", () => "Hello World!");
        app.MapControllers();

        app.Run();
    }
}