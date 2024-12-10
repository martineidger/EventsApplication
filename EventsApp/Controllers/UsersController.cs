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
using UseCases.Events;
using UseCases.Users;

namespace EventsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        #region constructor
        private readonly IValidator<RegistrationModel> _regValidator;
        private readonly IValidator<AuhorizationModel> _loginValidator;

        private readonly GetNotificationsListUseCase getNotificationsListUseCase;
        private readonly GetUserByIdUseCase getUserByIdUseCase;
        private readonly GetUserByNameUseCase getUserByNameUseCase;
        private readonly GetUsersFromEventUseCase getUsersFromEventUseCase;

        private readonly FollowUserUseCase followUserUseCase;
        private readonly UnfollowUserUseCase unfollowUserUseCase;

        public UsersController(
            IValidator<RegistrationModel> registrationValidator, 
            IValidator<AuhorizationModel> loginValidator, 
            GetUsersFromEventUseCase getUsersFromEventUseCase,
            GetUserByNameUseCase getUserByNameUseCase, 
            GetUserByIdUseCase getUserByIdUseCase,
            GetNotificationsListUseCase getNotificationsListUseCase,
            FollowUserUseCase followUserUseCase,
            UnfollowUserUseCase unfollowUserUseCase
           )
        {
            _loginValidator = loginValidator;
            _regValidator = registrationValidator;

            this.getNotificationsListUseCase = getNotificationsListUseCase;
            this.getUserByIdUseCase = getUserByIdUseCase;
            this.getUserByNameUseCase = getUserByNameUseCase;
            this.getUsersFromEventUseCase = getUsersFromEventUseCase;

            this.followUserUseCase = followUserUseCase;
            this.unfollowUserUseCase = unfollowUserUseCase;
        }
        #endregion

        #region get
        [Authorize(Policy ="AdminOnly")]
        [HttpGet("GetUsersFromEvent/{eventId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<User>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetUsersFromEvent(int eventId)
        {
            
            return Ok(await getUsersFromEventUseCase.ExecuteAsync(eventId));
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(User))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetByID(int id)
        {
            
            return Ok(await getUserByIdUseCase.ExecuteAsync(id));
        }

        [Authorize]
        [HttpGet("notifications/{userId}")]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Notification>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetNotificationsList(int id)
        {

            return Ok(await getNotificationsListUseCase.ExecuteAsync(id));
        }
        #endregion

        #region reg/del to event

        [Authorize]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [HttpPut("RegisterToEvent/{eventId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RegisterUserToEvent(int eventId, int userId)
        {
            await followUserUseCase.ExecuteAsync(eventId, userId);
            return Ok();
        }

        [Authorize]
        [ServiceFilter(typeof(IsUsersResourceFilter))]
        [HttpDelete("RemoveFromEvent/{eventId}/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveUserFromEvent(int eventId, int userId)
        {
            await unfollowUserUseCase.ExecuteAsync(eventId, userId);
            return Ok();
        }
        #endregion

    }
}