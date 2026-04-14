using Planora.DataAccess.Models;
using Planora.DTO.CategoryDTO;

namespace Planora.DataAccess.Mappers;

public static class CategoryMapping 
{
	public static CategoryDB ToEntity(CategoryDTO categoryDto) 
	{
		return new CategoryDB(
			name: categoryDto.Name, 
			hexColor: categoryDto.HexColor
		);
	}
	
	public static CategoryDTO ToDTO(CategoryDB categoryDb) {
		return new CategoryDTO(
			CategoryId: categoryDb.CategoryId.ToString(), 
			Name: categoryDb.Name, 
			HexColor: categoryDb.HexColor);
	}
}