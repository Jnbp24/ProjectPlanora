using System.Collections.Generic;
using System.Threading.Tasks;
using Planora.DTO.TaskDTO;

namespace Planora.DataAccess.Repositories.Task
{
    public interface ITaskRepository
    {
        Task<TaskDTO?> GetTaskById(int id);
        Task<IEnumerable<TaskDTO>> GetAllTasks();
        Task<TaskDTO> CreateTask(TaskDTO dto);
        Task<TaskDTO> UpdateTask(int id, TaskDTO dto);
        Task<TaskDTO> DeleteTask(int id);
        Task<TaskDTO> AssignUserToTask(int taskId, int userId);
    }
}
