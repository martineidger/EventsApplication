using AutoMapper;
using Events.Domain.Entities;
using Events.Domain.Extensions;
using Events.Domain.Interfaces;
using Events.DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Users
{
    public class GetNotificationsListUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GetNotificationsListUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NotificationDTO>> ExecuteAsync(int userId)
        {
            var user = await _unitOfWork.UserRepo.GetByIdAsync(userId)
                ?? throw new Exception($"no user with such id: {userId}");

            var notificationsList = _mapper.Map<IEnumerable<NotificationDTO>>(user.Notifications)
                ?? throw new Exception($"No notifications for this user: {user.Name}");

            return notificationsList;
        }
    }
}
