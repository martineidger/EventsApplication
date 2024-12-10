using AutoMapper;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCases.Events
{
    public class AddEventUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public AddEventUseCase(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Event> ExecuteAsync(EventDTO eventData)
        {
            if (_unitOfWork.EventRepo.GetEventByNameAsync(eventData.Name) != null)
                throw new Exception("Event with such name already exists");

            var newEvent = _mapper.Map<Event>(eventData);

            /*if (eventData.Image != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var fileExtension = Path.GetExtension(eventData.Image.FileName).ToLowerInvariant();

                if (!allowedExtensions.Contains(fileExtension))
                {
                    return BadRequest("Файл должен быть изображением (jpg, jpeg, png, gif).");
                }

                var title = eventData.Name;
                var words = title.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                 .Take(3)
                                 .Select(word => char.ToUpper(word[0]) + word.Substring(1).ToLower())
                                 .ToArray();
                var formattedTitle = string.Join("", words);

                var newFileName = $"{formattedTitle}{fileExtension}";
                var filePath = Path.Combine("wwwroot/images", newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    eventData.Image.CopyToAsync(stream);
                }

                newEvent.ImagePath = $"/images/{newFileName}";
            }*/

            await _unitOfWork.EventRepo.AddAsync(newEvent);
            await _unitOfWork.SaveChangesAsync();

            return newEvent;
        }
    }
}
