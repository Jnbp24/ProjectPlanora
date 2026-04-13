using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public class TaskRepository : ITaskRepository
{
    private readonly DatabaseContext _context;
    private ITaskRepository _taskRepositoryImplementation;

    public TaskRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<TaskDB> CreateTaskAsync(TaskDB task)
    {
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public Task<TaskDB> UpdateTaskAsync(string taskId, TaskDB task)
    {
        throw new NotImplementedException();
        //TODO: Might not need this method, since we can just update the entity and call SaveChangesAsync() in the service layer
        //Check best practice
    }

    public Task<TaskDB> DeleteTaskAsync(string taskId)
    {
        throw new NotImplementedException();
        //TODO: Might not need this method, since we can just update the entity and call SaveChangesAsync() in the service layer
        //Check best practice
    }

    public async Task<IEnumerable<TaskDB>> GetAllTasksAsync()
    {
        return await _context.Tasks.ToListAsync();
    }

    public async Task<TaskDB?> GetTaskByIdAsync(string id)
    {
        return await _context.Tasks.FindAsync(Guid.Parse(id));
    }

    public async System.Threading.Tasks.Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId)
    {
        // Need to create user relation between task and user for this implementation
        throw new NotImplementedException();
    }
}