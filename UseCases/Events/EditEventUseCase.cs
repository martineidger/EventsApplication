using AutoMapper;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class EditEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EditEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task ExecuteAsync(int curEventId, EventDTO newEventData)
        {
            if (_unitOfWork.EventRepo.GetByIdAsync(curEventId) == null)
                throw new Exception($"No event with such ID :{curEventId}");

            var newEvent = _mapper.Map<Event>(newEventData);
            newEvent.Id = curEventId;

            var entry = _unitOfWork.EventRepo.Update(newEvent);

            var modifiedProperties = entry.OriginalValues.Properties
                .Where(p => entry.Property(p.Name).IsModified)
                .ToDictionary(p => p.Name, p => entry.Property(p.Name).CurrentValue);

            foreach (var property in modifiedProperties)
            {
                _unitOfWork.EventRepo.OnEventChanged(curEventId, $"{property.Key} has been updated from to {property.Value}.");
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
