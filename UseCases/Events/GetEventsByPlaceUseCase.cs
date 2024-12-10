using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetEventsByPlaceUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventsByPlaceUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Event>> ExecuteAsync(string place)
        {
            return await _unitOfWork.EventRepo.GetEventsAsync(place) 
                ?? throw new Exception($"No events on this place: {place}");
        }
    }
}
