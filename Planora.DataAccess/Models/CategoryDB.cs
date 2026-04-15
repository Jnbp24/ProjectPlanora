using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Planora.DataAccess.Models;

public class CategoryDB 
{
	[Key]
	[Column("Id")]
	public required Guid CategoryId { get; set; }
	public string Name { get; set; }
	public string HexColor { get; set; }
	public bool Deleted { get; set; }
	public List<TaskDB> Tasks { get; set; } = [];

	public CategoryDB()
	{
	}
}