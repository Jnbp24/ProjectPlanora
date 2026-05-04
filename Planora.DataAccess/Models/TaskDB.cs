using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planora.DataAccess.Models;

public class TaskDB
{
    [Key]
    [Column("Id")]
    public required Guid TaskId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? Deadline { get; set; }
    public bool Deleted { get; set; }
    public bool Done { get; set; }
    public Guid? CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public CategoryDB? Category { get; set; }
    public Guid? CalenderYearId { get; set; }
    [ForeignKey("CalenderYearId")]
    public CalenderYearDB? CalenderYear { get; set; }
    public List<UserDB> Users { get; set; } = [];
    
    public TaskDB()
    {
    }
    
}