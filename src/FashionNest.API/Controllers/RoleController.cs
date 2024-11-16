using FashionNest.Application.Features.Roles.Queries.GetRoleById;
using FashionNest.Application.Features.Roles.Queries.GetRole;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FashionNest.Application.Features.Roles.Commands.CreateRole;
using FashionNest.Application.Features.Roles.Commands.UpdateRole;
using FashionNest.Application.Features.Roles.Commands.DeleteRole;
using FashionNest.Application.Common.Messaging;
using FashionNest.Domain.Common;
using FashionNest.Application.Features.Roles.Responses;

namespace FashionNest.API.Controllers
{
    //[Authorize]
    [Route("api/role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ISender sender;

        public RoleController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of all roles.
        /// </summary>
        /// <param name="pageIndex">Page index to start from, default is 1.</param>
        /// <param name="pageSize">Number of records per page, default is 10.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<RoleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await sender.Send(new GetRolesQuery(pageIndex, pageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific role by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<RoleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetRoleByIdQuery(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new role.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<RoleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateRoleCommand request) 
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing role.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<RoleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateRoleCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a role by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteRoleCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    } 
}
