using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Orders.Commands.CreateOrder;
using FashionNest.Application.Features.Orders.Commands.DeleteOrder;
using FashionNest.Application.Features.Orders.Commands.UpdateOrder;
using FashionNest.Application.Features.Orders.Queries.GetOrderById;
using FashionNest.Application.Features.Orders.Queries.GetOrders;
using FashionNest.Application.Features.Orders.Responses;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ISender sender;

        public OrderController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of all orders.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<OrderResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetOrdersQuery request)
        {
            var result = await sender.Send(new GetOrdersQuery(request.UserId, request.PageIndex, request.PageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific order by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetOrderByIdQuery(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<OrderResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateOrderCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing order.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(typeof(Result<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateOrderCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete an order by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteOrderCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
