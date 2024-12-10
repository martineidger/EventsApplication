using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Authentication
{
    public class RegistrationUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;

        public RegistrationUseCase(IUnitOfWork unitOfWork, IMapper mapper, IAdminUserService adminUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminUserService = adminUserService;
        }
        public async Task ExecuteAsync(RegistrationModel model)
        {
            if (await _unitOfWork.UserRepo.GetUserByEmailAsync(model.Email) != null)
                throw new Exception("user with such email already exists");

            var newUser = _mapper.Map<User>(model);
                      
            newUser.Role = _adminUserService.GetRole(newUser.Email);

            await _unitOfWork.UserRepo.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
