using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}
