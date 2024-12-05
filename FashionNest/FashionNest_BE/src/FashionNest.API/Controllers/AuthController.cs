using FashionNest.Application.Features.Auth.Commands.ChangePassword;
using FashionNest.Application.Features.Auth.Commands.ForgotPassword;
using FashionNest.Application.Features.Auth.Commands.Register;
using FashionNest.Application.Features.Auth.Commands.ResetPassword;
using FashionNest.Application.Features.Auth.Queries.Login;
using FashionNest.Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace FashionNest.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender sender;

        public AuthController(ISender sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(Result<LoginResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(LoginUserQuery request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(typeof(Result<RegisterUserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterUserCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand request)
        {
            var result = await sender.Send(request);
            if (result.IsFailure)
            {
                return BadRequest(new { Error = result.Error.Name });
            }
            return Ok(result.Value);
        }

        /// <summary>
        /// Change Password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("change-password")]
        [ProducesResponseType(typeof(Result<ChangePasswordResult>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand request)
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
