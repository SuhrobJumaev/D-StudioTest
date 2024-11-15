using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Todo.API.Intefaces;


namespace Todo.API.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/tasks")]
    [ApiVersion(Utils.API_VERSION_1)]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTaskAsync([FromBody] CreateTaskDto taskDto, CancellationToken token = default)
        {
            var userId = int.Parse(HttpContext.User.FindFirstValue("UserId")!);

            var response = await _service.CreateTaskAsync(taskDto, userId);

            if (response.Code == ApiErrorCode.Success)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTaskAsync([FromBody] UpdateTaskDto taskDto, CancellationToken token = default)
        {
            var response = await _service.UpdateTaskAsync(taskDto);

            if (response.Code == ApiErrorCode.Success)
                return Ok(response);

            return BadRequest(response);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTaskAsync([FromRoute] int id, CancellationToken token = default)
        {
            var result = await _service.DeleteTaskAsync(id);

            if (result.Code == ApiErrorCode.Success)
                return Ok(result);

            return BadRequest(result);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskUserDtoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult?> GetTaskAsync([FromRoute] int id, CancellationToken token = default)
        {

            var task = await _service.GetTaskByIdAsync(id);

            if (task is not null)
                return Ok(task);

            return NotFound();
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginationResponse<TaskUserDtoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(PaginationResponse<UserModel>), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(PaginationResponse<UserModel>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTasksAsync([FromQuery] UserQueryOptions queryOptions, CancellationToken token = default)
        {
            var userId = int.Parse(HttpContext.User.FindFirstValue("UserId")!);
            var paginationResponse = await _service.GetTasksAsync(queryOptions, userId, token);

            if (paginationResponse.Enttities.Any())
                return Ok(paginationResponse);

            return NotFound(paginationResponse);
        }
    }
}
