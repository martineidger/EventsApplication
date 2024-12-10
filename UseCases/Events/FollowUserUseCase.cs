using Events.Domain.Extensions;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class FollowUserUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public FollowUserUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(int eventId, int userId)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userId) ?? throw new Exception($"No user with such ID: {userId}");
            var curEvent = await _unitOfWork.EventRepo.GetByIdAsync(eventId) ?? throw new Exception($"No event with such ID :{eventId}");

            if (!curEvent.CanRegisterOnEvent())
                throw new Exception("it isn't possible to register for the event");

            curEvent.Users.Add(user);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
