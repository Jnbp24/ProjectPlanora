using System.ComponentModel.DataAnnotations;

namespace Planora.DataAccess.Models;

public class CategoryDB 
{
	public CategoryDB() 
	{
	}

	public CategoryDB(string name, string hexColor)
	{
		CategoryId = Guid.NewGuid();
		Name = name;
		HexColor = hexColor;
	}
	
	[Key]
	public Guid CategoryId { get; set; }
	public string Name { get; set; }
	public string HexColor { get; set; }
	public bool Deleted { get; set; }

}