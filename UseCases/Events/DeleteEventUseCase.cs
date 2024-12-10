using Events.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class DeleteEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteEventUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task ExecuteAsync(int eventId)
        {
            var entity = await _unitOfWork.EventRepo.GetByIdAsync(eventId);
            _unitOfWork.EventRepo.Delete(entity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
