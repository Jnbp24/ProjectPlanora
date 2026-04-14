using System.ComponentModel.DataAnnotations;

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

    [Key]
    public Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Deleted { get; set; }
    public CategoryDB? Category { get; set; } 

    public Guid? CategoryId { get; set; }
    public ICollection<TaskUserDB> TaskUsers { get; set; } = new List<TaskUserDB>();

}