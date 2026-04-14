using System.ComponentModel.DataAnnotations;

namespace Planora.DataAccess.Models;

public class CategoryDB 
{
	[Key]
	public Guid CategoryId { get; set; }
	public string Name { get; set; }
	public string HexColor { get; set; }
	public bool Deleted { get; set; }
	public List<TaskDB> Tasks { get; set; }

	public CategoryDB()
	{
	}
}