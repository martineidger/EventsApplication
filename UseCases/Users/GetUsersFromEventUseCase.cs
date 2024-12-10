using Events.Domain.Entities;
using Events.Domain.Extensions;
using Events.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Users
{
    public class GetUsersFromEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetUsersFromEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<User>> ExecuteAsync(int eventId)
        {
            var curEvent = await _unitOfWork.EventRepo.GetByIdAsync(eventId)
                ?? throw new Exception($"no event with such id: {eventId}");
            
            var usersList = curEvent.GetUsersFromEvent();
            if (usersList == null || !usersList.Any())
            {
                throw new Exception($"No subscribed users for this event: {curEvent.Name}");
            }
            return usersList;
        }
    }
}
