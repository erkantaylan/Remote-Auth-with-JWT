//https://www.youtube.com/watch?v=mgeuh8k3I4g

using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using Weather.Api.Identity;
using Weather.Api.Swagger;

namespace Weather.Api;

public static class MoviesProgram
{
    public static void Main(string[] args)
    {
        var jwtSettings = new JwtSettings(
            "http://localhost",
            "http://localhost",
            "VeryUnsecureWayToKeepTheKeyButItShouldBeLongEnoughToPassNow");

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddControllers();
        
        builder.Services.AddAuthentication(
                   o =>
                   {
                       o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                       o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                       o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                   })
               .AddJwtBearer(
                   o =>
                   {
                       o.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidIssuer = jwtSettings.Issuer,
                           ValidAudience = jwtSettings.Audience,
                           IssuerSigningKey =
                               new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true
                       };
                   });

        builder.Services.AddAuthorization(
            options =>
            {
                options.AddPolicy(
                    IdentityData.AdminUserPolicyName,
                    policyBuilder => policyBuilder.RequireClaim(IdentityData.AdminUserClaimName, "true"));
            });


        WebApplication app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();


        app.Run();
    }
}