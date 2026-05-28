namespace AMS.Repository;

public interface ITaskManagementRepository
{
    // CRUD operations for Task Management entity
    Task<IEnumerable<TaskManagement>> GetAllTasksAsync(CancellationToken cancellationToken);
    Task<TaskManagement> GetTaskByIdAsync(long id, CancellationToken cancellationToken);
    Task<TaskManagement> AddTaskAsync(TaskManagement taskManagement, CancellationToken cancellationToken);
    Task<TaskManagement> UpdateTaskAsync(TaskManagement taskManagement, CancellationToken cancellationToken);
    Task<TaskManagement> DeleteTaskAsync(long id, CancellationToken cancellationToken);
}
