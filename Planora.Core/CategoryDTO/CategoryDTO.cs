namespace Planora.DTO.CategoryDTO;

public class CategoryDTO {
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string HexColor { get; set; }

	public CategoryDTO() {
	}

	public CategoryDTO(Guid id, string name, string hexColor) {
		Id = id;
		Name = name;
		HexColor = hexColor;
	}
}