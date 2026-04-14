using System.ComponentModel.DataAnnotations;

namespace Planora.DataAccess.Models;

public class ProjectDB
{
	public ProjectDB()
	{
	}

    public ProjectDB(string title, string content)
    {
        ProjectId = Guid.NewGuid();
        Title = title;
        Content = content;
    }

    [Key]
    public Guid ProjectId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Deleted { get; set; }

}
