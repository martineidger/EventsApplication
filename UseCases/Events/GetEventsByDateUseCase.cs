using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetEventsByDateUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventsByDateUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Event>> ExecuteAsync(DateTime eventDate)
        {
            return await _unitOfWork.EventRepo.GetEventsAsync(eventDate) 
                ?? throw new Exception("No event on such date");
        }
    }
}
