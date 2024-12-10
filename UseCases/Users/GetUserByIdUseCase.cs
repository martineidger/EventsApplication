using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Users
{
    public class GetUserByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUserByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<User> ExecuteAsync(int id)
        {
            return await _unitOfWork.UserRepo.GetByIdAsync(id)
                ?? throw new Exception($"no user with such id: {id}");
        }
    }
}
