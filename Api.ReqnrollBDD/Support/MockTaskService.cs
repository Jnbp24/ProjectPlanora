using System;
using System.Collections.Generic;
using System.Text;
using Planora.DataAccess.Mappers;
using Planora.DTO.Task;

namespace Service.ReqnrollBDD.Support
{
	public class MockTaskService
	{
		public MockTaskService()
		{
		}

		public async Task<TaskDTO> CreateTaskAsync(TaskDTO taskDTO)
		{
			var taskDB = TaskMapping.ToEntity(taskDTO);
			return TaskMapping.ToDTO(taskDB);
		}
	}
}
