using Events.Authentications.AuthModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.Services.Intrfaces
{
    public interface ITokenService 
    {
        public (string accessToken, string refreshToken) GenerateTokens(GetTokenRequestModel model);
        public (string accessToken, string refreshToken) RefreshTokens(string refreshToken, GetTokenRequestModel model);
    }
}
