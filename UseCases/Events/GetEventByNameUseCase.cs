using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetEventByNameUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventByNameUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Event> ExecuteAsync(string name)
        {
            return await _unitOfWork.EventRepo.GetEventByNameAsync(name)
                ?? throw new Exception($"No event with such name: {name}");
        }
    }
}
