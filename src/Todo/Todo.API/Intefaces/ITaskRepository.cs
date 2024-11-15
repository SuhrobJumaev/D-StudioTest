namespace Todo.API.Intefaces
{
    public interface ITaskRepository
    {
        Task<TaskModel?> GetTaskByIdAsync(int taskId, CancellationToken token = default);
        Task<TaskUserDtoResponse?> GetTaskAsync(int taskId, CancellationToken token = default);
        Task<int> CreateTaskAsync(TaskModel task, CancellationToken token = default);
        Task<bool> UpdateTaskAsync(TaskModel task, CancellationToken token = default);
        Task<bool> DeleteTaskAsync(TaskModel task, CancellationToken token = default);
        Task<List<TaskUserDtoResponse>> GetTasksAsync(UserQueryOptions queryOptions, int userId, CancellationToken token = default);
        Task<int> GetCountTaskCountAsync(UserQueryOptions queryOptions, int userId, CancellationToken token = default);
    }
}
