using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Api.Jwt;

//https://youtu.be/bRtCifC6JsQ
public class JwtAuthenticationManager
{
    private readonly JwtSettings settings;

    public JwtAuthenticationManager(JwtSettings settings)
    {
        this.settings = settings;
    }

    public string Authanticate(Guid userId, string username)
    {
        string roles = JsonSerializer.Serialize(
            new[]
            {
                "magaza.admin",
                "medicine.pharmacist",
                "td.admin"
            });
        var tokenHandler = new JwtSecurityTokenHandler();
        byte[] tokenKey = Encoding.UTF8.GetBytes(settings.Key);
        var claimsIdentity = new ClaimsIdentity(
            new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, username),
                new(JwtRegisteredClaimNames.Sub, username),
                new("roles", roles)
            });
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = settings.Issuer,
            Audience = settings.Audience
        };

        SecurityToken? token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}