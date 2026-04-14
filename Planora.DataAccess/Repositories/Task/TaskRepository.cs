using Planora.DataAccess.Context;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
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
            throw new NotSupportedException($"Invalid id format: {id}");

        var task = await _dbContext.Tasks
            .Include(t => t.Category)
            .FirstOrDefaultAsync(t => t.TaskId == guid);

        if (task == null)
            throw new KeyNotFoundException($"Task {id} not found");

        return task;
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

        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
            ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");

        var task = await _dbContext.Tasks.FindAsync(tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        task.CategoryId = category.CategoryId;
        task.Category = category;
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

        var task = await _dbContext.Tasks.FindAsync(tGuid)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        // Ensure the task is actually assigned to the category we're "unassigning" from
        if (task.CategoryId != category.CategoryId)
            throw new InvalidOperationException(
                $"Task {taskId} is not assigned to category '{categoryName}' and therefore cannot be unassigned");

        task.CategoryId = defaultCategory.CategoryId;
        task.Category = defaultCategory;

        await _dbContext.SaveChangesAsync();
        return task;
    }
}