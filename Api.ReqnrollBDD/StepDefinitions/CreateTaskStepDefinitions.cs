using FluentAssertions;
using NUnit.Framework;
using Planora.Api.Services.Task;
using Planora.DTO.Task;
using Service.ReqnrollBDD.Support;

namespace Api.ReqnrollBDD.StepDefinitions
{
	[Binding]
	public sealed class CreateTaskStepDefinitions
	{
		private TaskDTO _Task;
		private TaskDTO _CreatedTask;
		private readonly MockTaskService _Service = new MockTaskService();

		[Given("User has filled in Task info")]
		public void UserHasFilledInTaskInfo()
		{
			_Task = new TaskDTO(
				null, 
				"Kapsejlads opslag", 
				"Instagram opslag, som skal gøre studerende opmærksom på at der er kapsejlads", 
				new DateTime()
			);
		}

		[When("User clicks Create Task button")]
		public async Task UserClicksCreateTaskButton()
		{
			_CreatedTask = await _Service.CreateTaskAsync(_Task);
		}

		[Then("Task is created")]
		public void TaskIsCreated()
		{
			_CreatedTask.TaskId.Should().NotBe(null);
		}
	}
}
