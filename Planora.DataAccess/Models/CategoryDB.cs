using Microsoft.EntityFrameworkCore;

namespace Planora.DataAccess.Models;

[PrimaryKey("Id")]
public class CategoryDB {
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string HexColor { get; set; }
	public bool Deleted { get; set; }

	public CategoryDB() {
	}

	public CategoryDB(string name, string hexColor) {
		Name = name;
		HexColor = hexColor;
	}
}