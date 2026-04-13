using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Task;

public class TaskRepository : ITaskRepository
{
    private readonly DatabaseContext _context;

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

    public async Task<IEnumerable<TaskDB>> GetAllTasksAsync()
    {
        return await _context.Tasks.Include(t => t.Category).ToListAsync();
    }

    public async Task<TaskDB?> GetTaskByIdAsync(string id)
    {
        if (!Guid.TryParse(id, out var guid))
            throw new KeyNotFoundException($"Invalid task id: {id}");

        var task = await _context.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TaskId == guid);

        if (task == null)
            throw new KeyNotFoundException($"Task {id} not found");

        return task;
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

    public async Task<TaskDB> AssignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId", nameof(taskId));
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name must be provided", nameof(categoryName));

        // Forced lowercase to avoid inconsistencies between user input and DB
        var nameNormalized = categoryName.Trim().ToLowerInvariant();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var task = await _context.Tasks.FindAsync(tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        task.CategoryId = category.CategoryId;
        task.Category = category;
        await _context.SaveChangesAsync();
        return task;
    }
    public async Task<TaskDB> UnassignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        if (!Guid.TryParse(taskId, out var tGuid))
            throw new ArgumentException("Invalid taskId", nameof(taskId));

        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name must be provided", nameof(categoryName));

        var nameNormalized = categoryName.Trim().ToLowerInvariant();

        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var defaultCategory = await _context.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == "default")
            ?? throw new KeyNotFoundException("Default category not found");

        var task = await _context.Tasks.FindAsync(tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        // Ensure the task is actually assigned to the category we're "unassigning" from
        if (task.CategoryId != category.CategoryId)
            throw new InvalidOperationException(
                $"Task {taskId} is not assigned to category '{categoryName}' and therefore cannot be unassigned");

        task.CategoryId = defaultCategory.CategoryId;
        task.Category = defaultCategory;

        await _context.SaveChangesAsync();
        return task;
    }
}