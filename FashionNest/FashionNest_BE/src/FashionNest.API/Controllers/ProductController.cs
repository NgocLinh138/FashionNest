using FashionNest.Application.Features.Products.Commands.CreateProduct;
using FashionNest.Application.Features.Products.Commands.DeleteProduct;
using FashionNest.Application.Features.Products.Commands.UpdateProduct;
using FashionNest.Application.Features.Products.Queries.GetProductByCategory;
using FashionNest.Application.Features.Products.Queries.GetProductById;
using FashionNest.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FashionNest.Domain.Common;
using FashionNest.Application.Features.Categories.Responses;
using FashionNest.Application.Common.Messaging;


namespace FashionNest.API.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ISender sender;

        public ProductController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of all products.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<CategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetProductsQuery request)
        {
            var result = await sender.Send(new GetProductsQuery(request.Filter, request.SortBy, request.SortOrderAscending, request.PageIndex, request.PageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get payment details by product ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetProductByIdQuery(id));

            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Get product details by category name.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet("category/{categoryName}")]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductsByCategoryName(string categoryName)
        {
            var result = await sender.Send(new GetProductByCategoryQuery(categoryName));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new product.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromForm] CreateProductCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing product.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<CategoryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateProductCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a product by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteProductCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
