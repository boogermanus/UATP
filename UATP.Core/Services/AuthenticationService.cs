using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UATP.Core.ApiModels;
using UATP.Core.Interfaces;


namespace UATP.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IConfiguration _configuration;
    private readonly string _username;
    private readonly string _password;
    private readonly byte[] _key;
    public AuthenticationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _username = _configuration["Jwt:Username"] ?? string.Empty;
        _password = _configuration["Jwt:Password"] ?? string.Empty;
        _key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);
    }
    
    public string? Authenticate(AuthenticationModel model)
    {
        if (model.Username != _username && model.Password != _password)
            return null;

        return GenerateToken();
    }

    private string GenerateToken()
    {
        var expires = int.Parse(_configuration["Jwt:Expires"] ?? string.Empty);
        var tokenHandler = new JwtSecurityTokenHandler();
        var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.NameId, _username),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddSeconds(expires),
            signingCredentials: credentials);

        return tokenHandler.WriteToken(token);
    }
}