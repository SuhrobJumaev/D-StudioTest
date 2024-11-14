using Asp.Versioning;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Todo.API.Dtos;
using Todo.API.Helpers;
using Todo.API.Intefaces;
using Todo.API.Services;

namespace Todo.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion(Utils.API_VERSION_1)]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class UserContoller : ControllerBase
    {
        private readonly IUserService _service;

        public UserContoller(IUserService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDto userDto, CancellationToken token = default)
        {
            var response = await _service.CreateUserAsync(userDto);

            if (response.Code == ApiErrorCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDto user, CancellationToken token = default)
        {
            var response = await _service.UpdateUserAsync(user);

            if (response.Code == ApiErrorCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] int id, CancellationToken token = default)
        {
            var result = await _service.DeleteUserAsync(id);
            
            if (result.Code == ApiErrorCode.Success)
                return Ok(result);

            return BadRequest(result);
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetUserAsync([FromRoute] int id, CancellationToken token = default)
        {
           
           var user = await _service.GetUserByIdAsync(id);

            if (user is not null)
                return Ok(user);

            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<UserModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PaginationResponse<UserModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(PaginationResponse<UserModel>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsersAsync([FromQuery] UserQueryOptions queryOptions, CancellationToken token = default)
        {
            var paginationResponse = await _service.GetUsersAsync(queryOptions, token);

            if (paginationResponse.Enttities.Any())
                return Ok(paginationResponse);

            return NotFound(paginationResponse);
        }

        
    }
}
