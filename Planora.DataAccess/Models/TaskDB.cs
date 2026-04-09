namespace Planora.DataAccess.Models;

internal class TaskDB
{
    public TaskDB()
    {
    }
    public Guid TaskId { get; set; }
    public string Content { get; set; }
    public string Title { get; set; }
    public bool Deleted { get; set; }
}