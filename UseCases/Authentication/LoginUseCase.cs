using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Authentication
{
    public class LoginUseCase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public LoginUseCase(IUnitOfWork unitOfWork, IMapper mapper, ITokenService tokenService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<TokenResponse> ExecuteAsync(AuhorizationModel model)
        {
            var user = await _unitOfWork.UserRepo.GetUserByEmailAsync(model.Email);
            
            

            var tokenRequest = _mapper.Map<GetTokenRequestModel>(user);
            var tokens = _tokenService.GenerateTokens(tokenRequest);
        }
    }
}
