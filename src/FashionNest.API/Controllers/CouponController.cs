using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Coupons.Commands.CreateCoupon;
using FashionNest.Application.Features.Coupons.Commands.DeleteCoupon;
using FashionNest.Application.Features.Coupons.Commands.UpdateCoupon;
using FashionNest.Application.Features.Coupons.Queries.GetCouponById;
using FashionNest.Application.Features.Coupons.Queries.GetCoupons;
using FashionNest.Application.Features.Coupons.Responses;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly ISender sender;

        public CouponController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of all coupons.
        /// </summary>
        /// <param name="pageIndex">Page index to start from, default is 1.</param>
        /// <param name="pageSize">Number of records per page, default is 10.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<CouponResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await sender.Send(new GetCouponsQuery(pageIndex, pageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get a specific coupon by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<CouponResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await sender.Send(new GetCouponByIdQuery(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new coupon.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Result<CouponResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateCouponCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing coupon.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<CouponResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateCouponCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Delete a coupon by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(typeof(Result<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await sender.Send(new DeleteCouponCommand(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
