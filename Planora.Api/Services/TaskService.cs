using System.Collections.Generic;
using Planora.DTO.TaskDTO;
using Planora.DataAccess.Repositories.Task;

namespace Planora.Api.Services
{
    public class TaskService
    {
        public readonly TaskRepository _repo;

        public TaskService(TaskRepository repo)
        {
            _repo = repo;
        }

        public IEnumerable<TaskDTO> GetAll() => _repo.;

        public TaskDTO? GetById(int id) => _repo.GetTaskById(id);

        public TaskDTO Create(TaskDTO dto) => _repo.CreateTask(dto);

        public void Update(int id, TaskDTO dto) => _repo.UpdateTask(id, dto);

        public void Delete(int id) => _repo.DeleteTask(id);
    }
}
