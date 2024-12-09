using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Interfaces;
using Events.DTOs.DTOs;
using Events.DTOs.HelperModels;
using Events.DTOs.HelperModels.Pagination;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        #region constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;
        private readonly IValidator<RegistrationModel> _regValidator;
        private readonly IValidator<AuhorizationModel> _loginValidator;
        private readonly IValidator<EventDTO> _eventValidator;
        private readonly string _secretKey;
        public EventsController(IMapper mapper, IUnitOfWork unitOfWork,
           IAdminUserService adminUserService, IConfiguration configuration,
           IValidator<RegistrationModel> regVal, IValidator<AuhorizationModel> loginVal, IValidator<EventDTO> eventVal)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminUserService = adminUserService;
            _loginValidator = loginVal;
            _regValidator = regVal;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _eventValidator = eventVal;
        }

        #endregion

        #region get
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Get(int id)
        {
            var sEvent = await _unitOfWork.EventRepo.GetByIdAsync(id);
            if (sEvent == null)
            {
                return NotFound();
            }
            return Ok(sEvent);
        }

        [HttpGet("allevents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] ItemPageParameters parameters)
        {
            var events = await _unitOfWork.EventRepo.GetAllAsync(parameters);
            if (events == null || !events.Any())
                return NotFound();
            return Ok(events);
        }

        [HttpGet("byname")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<IActionResult> GetByName([FromQuery] string name)
        {
            var sEvent = await _unitOfWork.EventRepo.GetEventByNameAsync(name);
            if (sEvent == null)
            {
                return NotFound();
            }
            return Ok(sEvent);
        }

        [HttpGet("bycategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByCategory([FromQuery] ItemPageParameters parameters, [FromQuery] string category)
        {
            if (Enum.TryParse(category, true, out EventCategory eventCategory))
            {
                var events =  await _unitOfWork.EventRepo.GetEventsAsync(parameters, eventCategory);
                return Ok(events);
            }
            else
            {
                return BadRequest("Invalid event category");
            }
        }

        [HttpGet("bydate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByDate([FromQuery] ItemPageParameters parameters, [FromQuery] DateTime date)
        {
            var events = await _unitOfWork.EventRepo.GetEventsAsync(parameters, date);
            if (events != null && events.Any())
                return Ok(events);
            else
                return BadRequest("No events found for the given date");
        }

        [HttpGet("byplace")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByPlace([FromQuery] ItemPageParameters parameters, [FromQuery] string place)
        {
            var events = await _unitOfWork.EventRepo.GetEventsAsync(parameters, place);
            if (events != null && events.Any())
                return Ok(events);
            else
                return BadRequest("No events found for the given place");
        }
        #endregion

        #region add
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddEvent([FromForm] EventDTO eventData)
        {
            var valResults = _eventValidator.Validate(eventData);
            if (!valResults.IsValid)
                return BadRequest(new { ValidationErrors = valResults.ToDictionary() });

            var newEvent = _mapper.Map<Event>(eventData);

            if (eventData.Image != null)
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
            }

            _unitOfWork.BeginTransaction();
            if(!(await _unitOfWork.EventRepo.AddAsync(newEvent)))
                return BadRequest("Event with such name already exists");
            _unitOfWork.Commit();

            return CreatedAtAction(nameof(GetByName), new { name = newEvent.Name }, newEvent);
        }

        #endregion

        #region update
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] UpdateEventModel newEvent)
        {
            var eventToUpdate = _mapper.Map<Event>(newEvent);
            _unitOfWork.BeginTransaction();
            if(await _unitOfWork.EventRepo.UpdateEventAsync(id, eventToUpdate) == 0)
                return NoContent();
            _unitOfWork.Commit();
            return Ok(_unitOfWork.EventRepo.GetByIdAsync(id));
        }
        #endregion

        #region delete
        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            _unitOfWork.BeginTransaction();
            if (await _unitOfWork.EventRepo.DeleteAsync(id))
            {
                _unitOfWork.Commit();
                return NoContent(); 
            }
            else
            {
                _unitOfWork.Rollback();
                return NotFound("No such event");
            }
        }
        #endregion

    }
}