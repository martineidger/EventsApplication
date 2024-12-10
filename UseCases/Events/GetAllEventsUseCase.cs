using Events.Domain.Entities;
using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetAllEventsUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllEventsUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> ExecuteAsync()
        {
            return await _unitOfWork.EventRepo.GetAllAsync();
        }
    }
}
