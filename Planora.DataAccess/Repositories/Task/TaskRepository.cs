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
            .FirstOrDefaultAsync(u => u.Id == uGuid)
            ?? throw new KeyNotFoundException($"User {userId} not found");
        task.Users.Add(user);
    
        // prevent duplicate users
        // if (task.TaskUsers.Any(tu => tu.UserId == uGuid))
        //     return task;
        
        await _dbContext.SaveChangesAsync();
        return task;
    }
    
    // public async Task<TaskDB> UnassignUserFromTaskAsync(string taskId, string userId)
    // {
    //     if (!Guid.TryParse(taskId, out var tGuid))
    //         throw new ArgumentException("Invalid taskId");
    //
    //     if (!Guid.TryParse(userId, out var uGuid))
    //         throw new ArgumentException("Invalid userId");
    //
    //     var task = await _dbContext.Tasks
    //         .Include(t => t.TaskUsers)
    //         .FirstOrDefaultAsync(t => t.TaskId == tGuid)
    //         ?? throw new KeyNotFoundException($"Task {taskId} not found");
    //
    //     var link = task.TaskUsers
    //         .FirstOrDefault(tu => tu.UserId == uGuid)
    //         ?? throw new KeyNotFoundException("User not assigned to this task");
    //
    //     task.TaskUsers.Remove(link);
    //
    //     await _dbContext.SaveChangesAsync();
    //     return task;
    // }
    //
    // public async Task<TaskDB> AssignCategoryToTaskByNameAsync(string taskId, string categoryName)
    // {
    //     if (!Guid.TryParse(taskId, out var tGuid))
    //         throw new ArgumentException("Invalid taskId", nameof(taskId));
    //
    //     if (string.IsNullOrWhiteSpace(categoryName))
    //         throw new ArgumentException("Category name must be provided", nameof(categoryName));
    //
    //     var nameNormalized = categoryName.Trim().ToLowerInvariant();
    //
    //     var category = await _dbContext.Categories
    //         .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
    //         ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");
    //
    //     var task = await _dbContext.Tasks
    //         .FirstOrDefaultAsync(t => t.TaskId == tGuid)
    //         ?? throw new KeyNotFoundException($"Task {taskId} not found");
    //
    //     task.CategoryId = category.CategoryId;
    //     task.Category = category;
    //
    //     await _dbContext.SaveChangesAsync();
    //     return task;
    // }
    //
    //
    // public async Task<TaskDB> UnassignCategoryToTaskByNameAsync(string taskId, string categoryName)
    // {
    //     if (!Guid.TryParse(taskId, out var tGuid))
    //         throw new ArgumentException("Invalid taskId", nameof(taskId));
    //
    //     if (string.IsNullOrWhiteSpace(categoryName))
    //         throw new ArgumentException("Category name must be provided", nameof(categoryName));
    //
    //     var nameNormalized = categoryName.Trim().ToLowerInvariant();
    //
    //     var category = await _dbContext.Categories
    //         .FirstOrDefaultAsync(c => c.Name.ToLower() == nameNormalized)
    //         ?? throw new KeyNotFoundException($"Category '{categoryName}' not found");
    //
    //     var defaultCategory = await _dbContext.Categories
    //         .FirstOrDefaultAsync(c => c.Name.ToLower() == "default")
    //         ?? throw new KeyNotFoundException("Default category not found");
    //
    //     var task = await _dbContext.Tasks
    //         .FirstOrDefaultAsync(t => t.TaskId == tGuid)
    //         ?? throw new KeyNotFoundException($"Task {taskId} not found");
    //
    //     if (task.CategoryId != category.CategoryId)
    //         throw new InvalidOperationException(
    //             $"Task {taskId} is not assigned to category '{categoryName}'");
    //
    //     if (task.CategoryId != category.CategoryId)
    //     {
    //         throw new InvalidOperationException(
    //             $"Task {taskId} is not assigned to category '{categoryName}'");
    //     }
    //
    //     task.CategoryId = defaultCategory.CategoryId;
    //     task.Category = defaultCategory;
    //
    //     await _dbContext.SaveChangesAsync();
    //     return task;
    // }
    //
    
    public Task<TaskDB> UnassignUserFromTaskAsync(string taskId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDB> AssignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        throw new NotImplementedException();
    }

    public Task<TaskDB> UnassignCategoryToTaskByNameAsync(string taskId, string categoryName)
    {
        throw new NotImplementedException();
    }
}