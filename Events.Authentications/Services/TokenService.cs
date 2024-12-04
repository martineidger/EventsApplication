using System;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
//using Duende.IdentityServer.Models;
using System.Text;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

public class TokenService : ITokenService
{
    private readonly string secretKey;
    private readonly JwtSecurityTokenHandler tokenHandler;

    private readonly int accessTokenLifeMin = 30;
    private readonly int refreshTokenLifeDays = 30;

    public TokenService(IConfiguration configuration)
    {
        this.secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        this.tokenHandler = new JwtSecurityTokenHandler();
    }
    
    public (string accessToken, string refreshToken) GenerateTokens(GetTokenRequestModel model)
    {
        //////access
        var key = System.Text.Encoding.ASCII.GetBytes(secretKey);
        var accessTokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role)
            }),
            Expires = DateTime.UtcNow.AddMinutes(accessTokenLifeMin),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
        var accessTokenStr = tokenHandler.WriteToken(accessToken);

        //////refresh

        Guid newGuid = Guid.NewGuid();
        string refreshTokenStr = newGuid.ToString("D");
        var refreshToken = new RefreshTokenModel()
        {
            Token = refreshTokenStr,
            UserId = model.Id.ToString(),
            ExpiresIn = DateTime.UtcNow.AddDays(refreshTokenLifeDays).ToString("o") // ISO 8601 формат
        };

        string tokenString = JsonConvert.SerializeObject(refreshToken);
        string base64Token = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(tokenString));
        Console.WriteLine("User refresh-token: " + base64Token);

        return (accessTokenStr, base64Token);
    }

    public (string accessToken, string refreshToken) RefreshTokens(string refreshToken, GetTokenRequestModel model)
    {
        if(!(ValidateRefreshToken(refreshToken) == model.Id))
            throw new SecurityTokenException("Token doesnt match current user's token");

        return GenerateTokens(model);
    }

    private int ValidateRefreshToken(string refreshToken)
    {
        byte[] bytes = Convert.FromBase64String(refreshToken);
        string jsonString = Encoding.UTF8.GetString(bytes);

        RefreshTokenModel refreshTokenModel = JsonConvert.DeserializeObject<RefreshTokenModel>(jsonString);
        if(refreshTokenModel == null)
            throw new SecurityTokenException("Error when converting refresh token: token cannot be null");

        DateTime expirationDate = DateTime.Parse(refreshTokenModel.ExpiresIn);
        if (DateTime.UtcNow > expirationDate)
            throw new SecurityTokenException($"Refresh token has been expired {refreshTokenModel.ExpiresIn}");

        return Convert.ToInt32(refreshTokenModel.UserId);

    }
}
