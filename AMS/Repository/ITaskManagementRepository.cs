using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

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

public class TaskManagementRepository : ITaskManagementRepository
{
    private readonly ApplicationDbContext _context;
    public TaskManagementRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<TaskManagement> AddTaskAsync(TaskManagement taskManagement, CancellationToken cancellationToken)
    {
        var data = await _context.taskManagements.AddAsync(taskManagement, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return taskManagement;
    }
    public async Task<TaskManagement> DeleteTaskAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.taskManagements.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.taskManagements.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Task not found");
        }
    }
    public async Task<IEnumerable<TaskManagement>> GetAllTasksAsync(CancellationToken cancellationToken)
    {
       var data = await _context.taskManagements.ToListAsync(cancellationToken);
        return data;
    }
    public async Task<TaskManagement> GetTaskByIdAsync(long id, CancellationToken cancellationToken)
    {
       var data = await _context.taskManagements.FindAsync(id, cancellationToken);
        return data;
    }
    public async Task<TaskManagement> UpdateTaskAsync(TaskManagement taskManagement, CancellationToken cancellationToken)
    {
        var data = await _context.taskManagements.FindAsync(taskManagement.Id, cancellationToken);
        if (data != null)
        {
            data.TaskTitle = taskManagement.TaskTitle;
            data.Description = taskManagement.Description;
            data.DueDate = taskManagement.DueDate;
            data.Status = taskManagement.Status;
            data.AdvocateId = taskManagement.AdvocateId;
            data.Advocate = taskManagement.Advocate;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Task not found");
        }
    }
}
