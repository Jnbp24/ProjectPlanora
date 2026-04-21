using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planora.DataAccess.Models;

public class CalenderYearDB
{
    [Key]
    [Column("Id")]
    public required Guid CalenderYearId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool Deleted { get; set; }
    private List<TaskDB> Tasks { get; set; }
    
    public CalenderYearDB()
    {
    }
}
