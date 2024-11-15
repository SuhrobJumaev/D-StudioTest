using Todo.API.Helpers.Mappers;
using Todo.API.Intefaces;
using Todo.API.Respositories;

namespace Todo.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IValidator<CreateTaskDto> _taskCreateValidator;
        private readonly IValidator<UpdateTaskDto> _taskUpdateValidator;

        public TaskService
            (ITaskRepository taskRepository,
            IValidator<CreateTaskDto> taskCreatedValidator,
            IValidator<UpdateTaskDto> taskUpdateValidator)
        {
            _taskRepository = taskRepository;
            _taskCreateValidator = taskCreatedValidator;
            _taskUpdateValidator = taskUpdateValidator;
        }

        public async Task<ApiResponse> CreateTaskAsync(CreateTaskDto createTaskDto, int createdId, CancellationToken token = default)
        {
            ApiResponse response = new();

            _taskCreateValidator.ValidateAndThrow(createTaskDto);

            var task = createTaskDto.MapCreateTaskDtoToTaskModel(createdId);

            int taskId = await _taskRepository.CreateTaskAsync(task, token);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (taskId > 0)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;

                response.Params.Add(new Param { Name = "TaskId", Value = taskId.ToString() });
            }

            return response;
        }

        public async Task<ApiResponse> UpdateTaskAsync(UpdateTaskDto updateTaskDto, CancellationToken token = default)
        {
            ApiResponse response = new();

            _taskUpdateValidator.ValidateAndThrow(updateTaskDto);

            TaskModel? task = await _taskRepository.GetTaskByIdAsync(updateTaskDto.Id, token);

            if (task is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.TaskNotFound;
                return response;
            }

            task.Name = updateTaskDto.Name;
            task.Description = updateTaskDto.Description;
            task.Status = updateTaskDto.Status;
            task.ModifiedDate = DateTime.Now;
            
            var result = await _taskRepository.UpdateTaskAsync(task);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (result)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;

                response.Params.Add(new Param { Name = "TaskId", Value = task.Id.ToString() });
            }

            return response;
        }

        public async Task<ApiResponse> DeleteTaskAsync(int id, CancellationToken token = default)
        {
            ApiResponse response = new();

            if (id >= 0)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.InvalidTaskId;
            }

            TaskModel? task = await _taskRepository.GetTaskByIdAsync(id, token);

            if (task is null)
            {
                response.Code = ApiErrorCode.Failed;
                response.Message = Utils.ErrorMessage.TaskNotFound;
                return response;
            }

            var result = await _taskRepository.DeleteTaskAsync(task, token);

            response.Code = ApiErrorCode.Failed;
            response.Message = Utils.ErrorMessage.Failed;

            if (result)
            {
                response.Code = ApiErrorCode.Success;
                response.Message = Utils.ErrorMessage.Success;
            }

            return response;
        }

        public async Task<TaskUserDtoResponse?> GetTaskByIdAsync(int id, CancellationToken token = default)
        {
            TaskUserDtoResponse? task = await _taskRepository.GetTaskAsync(id, token);

            if (task is null)
                return null;

            return task;
        }

        public async Task<PaginationResponse<TaskUserDtoResponse>> GetTasksAsync(UserQueryOptions userQueryOptions, int userId, CancellationToken token = default)
        {
            PaginationResponse<TaskUserDtoResponse> response = new();

            userQueryOptions.Limit = userQueryOptions.ItemsOfPage;
            userQueryOptions.Skip = userQueryOptions.Page == 1 ? 0 : (userQueryOptions.Page - 1) * userQueryOptions.Limit;

            IEnumerable<TaskUserDtoResponse> tasks = await _taskRepository.GetTasksAsync(userQueryOptions,userId, token);

            int countUsers = await _taskRepository.GetCountTaskCountAsync(userQueryOptions, userId, token);

            response.Enttities = tasks;
            response.CountPage = (int)Math.Ceiling(countUsers / (decimal)userQueryOptions.ItemsOfPage!);
            response.CurrentPage = userQueryOptions.Page;

            return response;
        }
    }
}
