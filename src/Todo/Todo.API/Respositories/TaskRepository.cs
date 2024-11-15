using Todo.API.Intefaces;

namespace Todo.API.Respositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskModel?> GetTaskByIdAsync(int taskId, CancellationToken token = default)
        {
            return await _context.Tasks
                                    .Where(t => t.Id == taskId)
                                    .FirstOrDefaultAsync(token);
        }

        public async Task<TaskUserDtoResponse?> GetTaskAsync(int taskId, CancellationToken token = default)
        {
            return await _context.Tasks
                                    .Where(t => t.Id == taskId)
                                    .Join(
                                        _context.Users,
                                        task => task.UserId,
                                        user => user.Id,
                                        (task, user) => new TaskUserDtoResponse
                                        {
                                            Id = task.Id,
                                            UserId = task.UserId,
                                            Name = task.Name,
                                            Description = task.Description,
                                            Status = task.Status,
                                            CreatedDate = task.CreatedDate,
                                            ModifiedDate = task.ModifiedDate,
                                            UserEmail = user.Email
                                        })
                                    .FirstOrDefaultAsync(token);
        }

        public async Task<int> CreateTaskAsync(TaskModel task, CancellationToken token = default)
        {
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync(token);

            return task.Id;
        }

        public async Task<bool> UpdateTaskAsync(TaskModel task, CancellationToken token = default)
        {
            _context.Tasks.Update(task);

            await _context.SaveChangesAsync(token);

            return true;
        }

        public async Task<bool> DeleteTaskAsync(TaskModel task, CancellationToken token = default)
        {
            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync(token);

            return true;
        }

        public async Task<List<TaskUserDtoResponse>> GetTasksAsync(UserQueryOptions queryOptions, int userId, CancellationToken token = default)
        {
            var query = _context.Tasks
                .Where(t => string.IsNullOrEmpty(queryOptions.Search) || t.Name.Contains(queryOptions.Search))
                .Where(t => t.UserId == userId)
                .Join(
                    _context.Users,
                    task => task.UserId,
                    user => user.Id,
                    (task, user) => new TaskUserDtoResponse
                    {
                        Id = task.Id,
                        UserId = task.UserId,
                        Name = task.Name,
                        Description = task.Description,
                        Status = task.Status,
                        CreatedDate = task.CreatedDate,
                        ModifiedDate = task.ModifiedDate,
                        UserEmail = user.Email
                    })
                .OrderByDescending(u => u.Id)
                .Skip((int)queryOptions.Skip!)
                .Take((int)queryOptions.ItemsOfPage!);
                

            return await query.ToListAsync(token);
        }

        public async Task<int> GetCountTaskCountAsync(UserQueryOptions queryOptions,int userId, CancellationToken token = default)
        {
            return await _context.Tasks
                .Where(t => string.IsNullOrEmpty(queryOptions.Search) || t.Name.Contains(queryOptions.Search))
                .Where(t => t.UserId == userId)
                .Join(
                    _context.Users,
                    task => task.UserId,        
                    user => user.Id,            
                    (task, user) => new { task, user } 
                )
                .CountAsync(token);
        }
    }
}
