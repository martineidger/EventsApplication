using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class GetFreePlacesCountForEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetFreePlacesCountForEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> ExecuteAsync(int id)
        {
            var curEvent = await _unitOfWork.EventRepo.GetByIdAsync(id)
                ?? throw new Exception($"no event with such id: {id}");

            return curEvent.FreePlacesCount();
        }
    }
}
