namespace Todo.API.Helpers.Mappers
{
    public static class TaskMapper
    {
        public static TaskModel MapCreateTaskDtoToTaskModel(this CreateTaskDto createTaskDto, int userId)
        {
            return new()
            {
                Name = createTaskDto.Name,
                Description = createTaskDto.Description,
                UserId = userId,
            };
        }
    }
}
