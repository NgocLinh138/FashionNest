using FashionNest.Application.Common.Messaging;
using FashionNest.Application.Features.Payments.Commands.VnPay.CreatePaymentVnPay;
using FashionNest.Application.Features.Payments.Commands.VnPay.UpdatePayment;
using FashionNest.Application.Features.Payments.Queries.GetPaymentById;
using FashionNest.Application.Features.Payments.Queries.GetPaymentByUserId;
using FashionNest.Application.Features.Payments.Queries.GetPayments;
using FashionNest.Application.Features.Payments.Responses;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISender sender;

        public PaymentController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Get a paginated list of all payments.
        /// </summary>
        /// <param name="pageIndex">Page index to start from, default is 1.</param>
        /// <param name="pageSize">Number of records per page, default is 10.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(Result<PaginatedResult<PaymentResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10)
        {
            var result = await sender.Send(new GetPaymentsQuery(pageIndex, pageSize));
            return Ok(result);
        }

        /// <summary>
        /// Get payment details by payment ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(Result<PaymentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentById(Guid id)
        {
            var result = await sender.Send(new GetPaymentByIdQuery(id));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Get payments by user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(Result<PaymentResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPaymentByUserId(Guid userId)
        {
            var result = await sender.Send(new GetPaymentByUserIdQuery(userId));
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Create a new VnPay payment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("VNPay")]
        [ProducesResponseType(typeof(Result<CreatePaymentVnPayResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Payments([FromBody] CreatePaymentVnPayCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Update an existing payment.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Result<PaymentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdatePayment([FromBody] UpdatePaymentCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }
    }
}
