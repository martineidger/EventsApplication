using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Users
{
    public class GetUserByNameUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByNameUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> ExecuteAsync(string userName)
        {
            return await _unitOfWork.UserRepo.GetUserByNameAsync(userName)
                ?? throw new Exception($"no user with such name: {userName}");
        }
    }
}
