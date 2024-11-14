using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Todo.API.Dtos;
using Todo.API.Helpers;
using Todo.API.Intefaces;

namespace Todo.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/identity")]
    [ApiVersion(Utils.API_VERSION_1)]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _service;

        public IdentityController(IIdentityService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, CancellationToken token)
        {
            ApiResponse response = await _service.Login(loginDto, token);

            if (response.Code == ApiErrorCode.Failed)
                return NotFound();

            return Ok(response);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto, CancellationToken token = default)
        {
            ApiResponse response = await _service.RefreshToken(refreshTokenDto, token);

            if (response.Code == ApiErrorCode.Failed)
                return NotFound();

            return Ok(response);
        }
    }
}
