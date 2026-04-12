namespace Planora.DataAccess.Models;

public class TaskDB
{
    public TaskDB()
    {
    }
    
    public TaskDB(string title, string content)
    {
        TaskId = Guid.NewGuid();
        Title = title;
        Content = content;
    }

    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Deleted { get; set; }
}