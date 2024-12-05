using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Users.Commands.UpdateUser;
using FashionNest.Application.Features.Users.Commands.UpdateUserStatus;
using FashionNest.Application.Features.Users.Queries.GetUserById;
using FashionNest.Application.Features.Users.Queries.GetUsers;
using FashionNest.Application.Features.Users.Responses;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ISender sender;

        public UserController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of users.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<UserResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetUsersQuery request) 
        {
            var result = await sender.Send(new GetUsersQuery(request.Filter, request.SortBy, request.SortOrderAscending, request.IsActive, request.PageIndex, request.PageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific user by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetUserByIdQuery(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateUserCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a user by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteUserCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
