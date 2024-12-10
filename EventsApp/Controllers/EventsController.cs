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
using UseCases.Events;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        #region constructor
        private readonly AddEventUseCase addEventUseCase;
        private readonly DeleteEventUseCase deleteEventUseCase;
        private readonly EditEventUseCase editEventUseCase;
        private readonly FollowUserUseCase followUserUseCase;
        private readonly UnfollowUserUseCase unfollowUserUseCase;
        private readonly GetAllEventsUseCase getAllEventsUseCase;
        private readonly GetEventsByCategoryUseCase getEventsByCategoryUseCase;
        private readonly GetEventsByDateUseCase getEventsByDateUseCase;
        private readonly GetEventsByPlaceUseCase getEventsByPlaceUseCase;
        private readonly GetEventByIdUseCase getEventByIdUseCase;
        private readonly GetEventByNameUseCase getEventByNameUseCase;
        private readonly GetFreePlacesCountForEventUseCase getFreePlacesCountForEventUseCase;

        private readonly IValidator<RegistrationModel> _regValidator;
        private readonly IValidator<AuhorizationModel> _loginValidator;
        private readonly IValidator<EventDTO> _eventValidator;
        public EventsController(
            IValidator<RegistrationModel> regVal,
            IValidator<AuhorizationModel> loginVal,
            IValidator<EventDTO> eventVal,
            AddEventUseCase addEventUseCase,
            DeleteEventUseCase deleteEventUseCase,
            EditEventUseCase editEventUseCase,
            FollowUserUseCase followUserUseCase,
            UnfollowUserUseCase unfollowUserUseCase,
            GetAllEventsUseCase getAllEventsUseCase,
            GetEventsByCategoryUseCase getEventsByCategoryUseCase,
            GetEventsByDateUseCase getEventsByDateUseCase,
            GetEventsByPlaceUseCase getEventsByPlaceUseCase,
            GetEventByIdUseCase getEventByIdUseCase, 
            GetEventByNameUseCase getEventByNameUseCase,
            GetFreePlacesCountForEventUseCase getFreePlacesCountForEventUseCase
            ) 
        {
            _loginValidator = loginVal;
            _regValidator = regVal;
            _eventValidator = eventVal;

            this.addEventUseCase = addEventUseCase;
            this.deleteEventUseCase = deleteEventUseCase;
            this.editEventUseCase = editEventUseCase;
            this.followUserUseCase = followUserUseCase;
            this.unfollowUserUseCase = unfollowUserUseCase;
            this.getAllEventsUseCase = getAllEventsUseCase;
            this.getEventsByCategoryUseCase = getEventsByCategoryUseCase;
            this.getEventsByDateUseCase = getEventsByDateUseCase;
            this.getEventsByPlaceUseCase = getEventsByPlaceUseCase;
            this.getEventByIdUseCase = getEventByIdUseCase;
            this.getEventByNameUseCase = getEventByNameUseCase;
            this.getFreePlacesCountForEventUseCase = getFreePlacesCountForEventUseCase;
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
            return Ok(await getEventByIdUseCase.ExecuteAsync(id));
        }

        [HttpGet("allevents")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] ItemPageParameters parameters)
        {
            return Ok(await getAllEventsUseCase.ExecuteAsync());
        }

        [HttpGet("byname")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async  Task<IActionResult> GetByName([FromQuery] string name)
        {
            return Ok(await getEventByNameUseCase.ExecuteAsync(name));
        }

        [HttpGet("bycategory")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByCategory([FromQuery] ItemPageParameters parameters, [FromQuery] string category)
        {
            return Ok(await getEventsByCategoryUseCase.ExecuteAsync(category));
        }

        [HttpGet("bydate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByDate([FromQuery] ItemPageParameters parameters, [FromQuery] DateTime date)
        {
            return Ok(await getEventsByDateUseCase.ExecuteAsync(date));
        }

        [HttpGet("byplace")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByPlace([FromQuery] ItemPageParameters parameters, [FromQuery] string place)
        {
            return Ok(await getEventsByPlaceUseCase.ExecuteAsync(place));
        }

        [HttpGet("free-places/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Event>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetEventsByPlace(int eventId)
        {
            return Ok(await getFreePlacesCountForEventUseCase.ExecuteAsync(eventId));
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
            await _eventValidator.ValidateAndThrowAsync(eventData);

            return Ok(await addEventUseCase.ExecuteAsync(eventData));
        }

        #endregion

        #region update
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Event))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateEvent(int id, [FromBody] EventDTO newEvent)
        {
            await _eventValidator.ValidateAndThrowAsync(newEvent);

            await editEventUseCase.ExecuteAsync(id, newEvent);

            return Ok();
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
            await deleteEventUseCase.ExecuteAsync(id);
            return Ok();
        }
        #endregion

    }
}