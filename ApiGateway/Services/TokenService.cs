using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using LoginProto;
using Microsoft.IdentityModel.Tokens;

namespace ApiGateway.Services;

public class TokenService
{
    internal const string Key = "ATestSecretKeyExample123@@!###!@@#!$qweqweqweqwe";
    public static readonly TimeSpan TokenLifeTime = TimeSpan.FromMinutes(15);

    public (string, Timestamp) GenerateToken(LoginRequest request)
    {
        var h = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Key);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(JwtRegisteredClaimNames.Sub, request.Email),
            new(JwtRegisteredClaimNames.Email, request.Email),
        };

        var expires = DateTime.UtcNow.Add(TokenLifeTime);

        var tokenDesc = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expires,
            Issuer = "WebStoreApp",
            Audience = "WebStoreApp",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256)
        };

        var token = h.CreateToken(tokenDesc);
        return (h.WriteToken(token), Timestamp.FromDateTime(expires));
    }
}