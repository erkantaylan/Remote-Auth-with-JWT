namespace Weather.Api;

public class JwtSettings
{
    public JwtSettings(string issuer, string audience, string key)
    {
        Issuer = issuer;
        Audience = audience;
        Key = key;
    }

    public string Issuer { get; }
    public string Audience { get; }
    public string Key { get; }
}