using Events.Domain.Entities;
using Events.Domain.Interfaces;
using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetEventsByCategoryUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventsByCategoryUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Event>> ExecuteAsync(string category)
        {
            if (!Enum.TryParse(category, true, out EventCategory eventCategory))
            {
                throw new Exception("Invalid event category");
            }
            return await _unitOfWork.EventRepo.GetEventsAsync(eventCategory) 
                ?? throw new Exception($"No event in this category: {category}");
        }

    }
}
