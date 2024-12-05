using FashionNest.Application.Common.Exceptions;
using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Categories.Commands.CreateCategory;
using FashionNest.Application.Features.Categories.Commands.DeleteCategory;
using FashionNest.Application.Features.Categories.Commands.UpdateCategory;
using FashionNest.Application.Features.Categories.Queries.GetCategories;
using FashionNest.Application.Features.Categories.Queries.GetCategoryById;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ISender sender;

        public CategoryController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of categories.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<CategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetCategoriesQuery request)
        {
            var result = await sender.Send(new GetCategoriesQuery(request.Filter, request.SortBy, request.SortOrderAscending, request.PageIndex, request.PageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific category by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetCategoryByIdQuery(id));

            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }

            return Ok(result);
        }

        /// <summary>
        /// Create a new category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateCategoryCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing category.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateCategoryCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a category by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteCategoryCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
