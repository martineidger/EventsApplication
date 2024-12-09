using AutoMapper;
using Duende.IdentityServer.Models;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.Persistence.Repositories;
using FluentValidation;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static Duende.IdentityServer.Events.TokenIssuedSuccessEvent;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class RegistrationController : Controller
    {
        #region constructors
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;
        private readonly IValidator<RegistrationModel> _regValidator;
        private readonly IValidator<AuhorizationModel> _loginValidator;
        private string secretKey;
        private readonly ITokenService _tokenService;

        public RegistrationController(IMapper mapper, IUnitOfWork unitOfWork,
            IAdminUserService adminUserService, IConfiguration configuration,
            IValidator<RegistrationModel> reg, IValidator<AuhorizationModel> login,
            ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminUserService = adminUserService;
            _loginValidator = login;
            _regValidator = reg;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _tokenService = tokenService;
        }
        #endregion

        #region tokens
        private void SetAccessTokenCookie(string accessToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.Strict, 
                Expires = DateTimeOffset.UtcNow.AddMinutes(30) 
            };

            Response.Cookies.Append("AccessToken", accessToken, cookieOptions);
        }
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        { 
            byte[] bytes = Convert.FromBase64String(refreshToken);
            string jsonString = Encoding.UTF8.GetString(bytes);

            RefreshTokenModel refreshTokenModel = JsonConvert.DeserializeObject<RefreshTokenModel>(jsonString);
            var tUser = await _unitOfWork.UserRepo.GetByIdAsync(Convert.ToInt32(refreshTokenModel.UserId));

            if (!string.Equals(tUser.Token, refreshToken))
                return BadRequest("Invalid refresh token: tokens aren't equals");

            var newToken = _tokenService.RefreshToken(refreshToken,
                new GetTokenRequestModel()
                {
                    Email = tUser.Email,
                    Id = tUser.Id,
                    Role = tUser.Role
                });

            //_unitOfWork.UserRepo.SetRefreshTokenAsync(newTokens.RefreshToken, tUser.Email);
            return Ok(new { AccessToken = newToken });
        }
        #endregion

        #region registration

        [HttpPost("registration")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Registration([FromBody] RegistrationModel user)
        {
            if (user == null)
                return BadRequest("User cannot be null");

            if (!(await _unitOfWork.UserRepo.RegisterAsync(user)))
                return BadRequest("Such user already exists");

            return Ok(new { User = user }); 
        }
        #endregion

        #region login
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] AuhorizationModel user)
        {
            if (user == null)
                return BadRequest("User cannot be null");

            var tokens = await _unitOfWork.UserRepo.LoginAsync(user);
            if (tokens == null)
                return Unauthorized("Invalid username or password");

            _unitOfWork.UserRepo.SetRefreshTokenAsync(tokens.RefreshToken, user.Email);
            SetAccessTokenCookie(tokens.AccessToken);

            return Ok(new { AccesToken = tokens.AccessToken , RefreshToken = tokens.RefreshToken});
        }

        #endregion
    }

}

