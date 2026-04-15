using Planora.DataAccess.Context;

using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public class TaskRepository : Repository<TaskDB>, ITaskRepository
{

    public TaskRepository(DatabaseContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<TaskDB>> GetAllAsync()
    {
        return await _dbContext.Tasks.Include(t => t.Category).ToListAsync();
    }

    public override async Task<TaskDB> GetByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new ArgumentException("Invalid taskId", nameof(id));

        var task = await _dbContext.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TaskId == guid);

        if (task == null)
            throw new KeyNotFoundException($"Task {id} not found");

        return task;
    }

    public async Task<TaskDB> AssignUserToTaskAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId");
    
        if (!Guid.TryParse(userId, out var uGuid))
            throw new ArgumentException("Invalid userId");
    
        var task = await _dbContext.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == uGuid)
            ?? throw new KeyNotFoundException($"User {userId} not found");
        task.Users.Add(user);
    
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskDB> UnassignUserFromTaskAsync(string taskId, string userId)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId");

        if (!Guid.TryParse(userId, out var uGuid))
            throw new ArgumentException("Invalid userId");

        var task = await _dbContext.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == uGuid)
            ?? throw new KeyNotFoundException($"User {userId} not found");
        task.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskDB> AssignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId", nameof(taskId));

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name must be provided", nameof(categoryName));

        var nameNormalized = categoryName.Trim().ToLowerInvariant();

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        category.Tasks.Add(task);

        await _dbContext.SaveChangesAsync();
        return task;
    }
    public async Task<TaskDB> UnassignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId", nameof(taskId));

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name must be provided", nameof(categoryName));

        var nameNormalized = categoryName.Trim().ToLowerInvariant();

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var defaultCategory = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == "default")
            ?? throw new KeyNotFoundException("Default category not found");

        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        // When removing a task from it's category, it should auto-assign back to the default category
        category.Tasks.Remove(task);
        task.Category = defaultCategory;

        await _dbContext.SaveChangesAsync();
        return task;
    }
}