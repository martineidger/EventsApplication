using AutoMapper;
using Events.Authentications.AuthModels;
using Events.Authentications.CustomAuthorization;

//using Events.Authentications.Services.Interfaces;
using Events.Authentications.Services.Intrfaces;
using Events.Domain.Entities;
using Events.Domain.Extensions;
using Events.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region constructor
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAdminUserService _adminUserService;
        private readonly IValidator<RegistrationModel> _regValidator;
        private readonly IValidator<AuhorizationModel> _loginValidator;
        private readonly string _secretKey;

        public UsersController(IMapper mapper, IUnitOfWork unitOfWork,
            IAdminUserService adminUserService, IConfiguration configuration,
            IValidator<RegistrationModel> reg, IValidator<AuhorizationModel> login)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _adminUserService = adminUserService;
            _loginValidator = login;
            _regValidator = reg;
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        #endregion

        #region get
        [Authorize(Policy ="AdminOnly")]
        [HttpGet("GetUsersFromEvent/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetUsersFromEvent(int eventId)
        {
            var curEvent = _unitOfWork.EventRepo.GetById(eventId);
            if (curEvent == null)
            {
                return NotFound("No such event");
            }
            var usersList = curEvent.GetUsersFromEvent();
            if (usersList == null || !usersList.Any())
            {
                return NotFound($"No subscribed users for this event: {curEvent.Name}");
            }
            return Ok(usersList);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetByID(int id)
        {
            var user = _unitOfWork.UserRepo.GetById(id);
            if (user == null)
            {
                return NotFound("No such user");
            }
            return Ok(user);
        }

        [Authorize]
        [HttpGet("notifications/{userId}")]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetNotificationsList(int userId)
        {
            var user = _unitOfWork.UserRepo.GetById(userId);
            if (user == null)
            {
                return NotFound("No such user");
            }

            var notificationsList = user.ShowNotifications();
            if (notificationsList == null || !notificationsList.Any())
            {
                return NotFound($"No notifications for this user: {user.Name}");
            }
            return Ok(notificationsList);
        }
        #endregion

        #region reg/del to event

        [Authorize]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [HttpPut("RegisterToEvent/{eventId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult RegisterUserToEvent(int eventId, int userId)
        {
            var success = _unitOfWork.EventRepo.RegisterUserOnEvent(eventId, userId);
            if (!success)
            {
                return NotFound("Cannot register this user");
            }
            return Ok(success);
        }

        [Authorize]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [HttpDelete("RemoveFromEvent/{eventId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult RemoveUserFromEvent(int eventId, int userId)
        {
            var success = _unitOfWork.EventRepo.RemoveUserFromEvent(eventId, userId);
            if (!success)
            {
                return NotFound("Cannot remove this user from event");
            }
            return Ok(success);
        }
        #endregion

    }
}