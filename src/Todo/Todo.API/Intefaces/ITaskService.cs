namespace Todo.API.Intefaces
{
    public interface ITaskService
    {
        Task<ApiResponse> CreateTaskAsync(CreateTaskDto createTaskDto, int createdId, CancellationToken token = default);
        Task<ApiResponse> UpdateTaskAsync(UpdateTaskDto updateTaskDto, CancellationToken token = default);
        Task<ApiResponse> DeleteTaskAsync(int id, CancellationToken token = default);
        Task<TaskUserDtoResponse?> GetTaskByIdAsync(int id, CancellationToken token = default);
        Task<PaginationResponse<TaskUserDtoResponse>> GetTasksAsync(UserQueryOptions userQueryOptions, int userId, CancellationToken token = default);
    }
}
