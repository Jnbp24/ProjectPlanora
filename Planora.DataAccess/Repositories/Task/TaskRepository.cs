using Planora.DataAccess.Context;

using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Models;
using Planora.DTO.Task;

namespace Planora.DataAccess.Repositories.Task;

public class TaskRepository : Repository<TaskDB>, ITaskRepository
{

    public TaskRepository(DatabaseContext context) : base(context)
    {
    }

    public override async Task<IEnumerable<TaskDB>> GetAllAsync()
    {
        return await _dbContext.Tasks.Include(t => t.Category).Include(t => t.Users).ToListAsync();
    }

    public override async Task<TaskDB> GetByIdAsync(Guid taskId)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TaskId == taskId);

        if (task == null)
            throw new KeyNotFoundException($"Task {taskId} not found");

        return task;
    }

    public async Task<TaskDB> AssignUserAsync(Guid taskId, Guid userId)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == userId)
            ?? throw new KeyNotFoundException($"User {userId} not found");
        if (task.Users.Contains(user))
        {
            throw new InvalidOperationException($"User {userId} is already assigned to task {taskId}");
        }
        task.Users.Add(user);
    
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskDB> UnassignUserAsync(Guid taskId, Guid userId)
    {
        var task = await _dbContext.Tasks
            .Include(t => t.Users)
            .FirstOrDefaultAsync(t => t.TaskId == taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.UserId == userId)
            ?? throw new KeyNotFoundException($"User {userId} not found");
        task.Users.Remove(user);

        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskDB> AssignCategoryAsync(Guid taskId, string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name must be provided", nameof(categoryName));

        var nameNormalized = categoryName.Trim().ToLowerInvariant();

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower().Equals(nameNormalized))
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var task = await _dbContext.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        category.Tasks.Add(task);

        await _dbContext.SaveChangesAsync();
        return task;
    }
    public async Task<TaskDB> UnassignCategoryAsync(Guid taskId, string categoryName)
    {
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
            .FirstOrDefaultAsync(t => t.TaskId == taskId)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        // When removing a task from it's category, it should auto-assign back to the default category
        category.Tasks.Remove(task);
        task.Category = defaultCategory;

        await _dbContext.SaveChangesAsync();
        return task;
    }

}