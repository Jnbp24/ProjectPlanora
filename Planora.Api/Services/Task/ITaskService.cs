using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Planora.DataAccess.Models;
using Planora.DTO.TaskDTO;

namespace Planora.Api.Services.Task
{
    public interface ITaskService
    {
        Task<TaskDTO> CreateAsync(TaskDTO dto);
        Task<TaskDTO> UpdateAsync(string taskId, TaskDTO dto);
        Task<IEnumerable<TaskDTO>> GetAllAsync();
        Task<TaskDTO?> GetByIdAsync(string taskId);
        Task<TaskDTO> DeleteAsync(string taskId);
        Task<TaskDTO> AssignCategoryAsync(string taskId, string categoryName);
        Task<TaskDTO> UnassignCategoryAsync(string taskId, string categoryName);
        Task<TaskDTO> AssignUserAsync(string taskId, string userId);
        Task<TaskDTO> UnassignUserAsync(string taskId, string userId);

    }
}
