using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetEventByIdUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetEventByIdUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Event> ExecuteAsync(int id)
        {
            return await _unitOfWork.EventRepo.GetByIdAsync(id) 
                ?? throw new Exception($"No event with such id: {id}");
        }
    }
}
