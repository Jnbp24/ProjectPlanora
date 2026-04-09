using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Mappers;
using Planora.DataAccess.Models;
using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Repositories.Task
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseContext _context;

        public TaskRepository(DatabaseContext context)
        {
            _context = context;
        }


        public async Task<TaskDTO> CreateTask(TaskDTO dto)
        {
            var entity = TaskMapping.ToEntity(dto);
            _context.Tasks.Add(entity);
            await _context.SaveChangesAsync();

            return TaskMapping.ToDTO(entity);
        }

        public async Task<TaskDTO> DeleteTask(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                throw new Exception("Task not found");

            task.Deleted = true;
            await _context.SaveChangesAsync();

            return TaskMapping.ToDTO(task);

        }
        public async Task<TaskDTO> UpdateTask(Guid id, TaskDTO dto)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                throw new Exception("Task not found");

            // We only want to update the title and content for the task
            task.Title = dto.Title;
            task.Content = dto.Content;

            await _context.SaveChangesAsync();

            return TaskMapping.ToDTO(task);
        }

        public async Task<IEnumerable<TaskDTO>> GetAllTasks()
        {
            var tasks = await _context.Tasks.ToListAsync();

            return tasks.Select(TaskMapping.ToDTO);
        }

        public async Task<TaskDTO?> GetTaskById(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
                throw new Exception("Task not found");

            return TaskMapping.ToDTO(task);
        }

        public Task<TaskDTO> AssignUserToTask(Guid taskId, int userId)
        {
            // Need to create user relation between task and user for this implementation
            throw new NotImplementedException();
        }
    }
}
