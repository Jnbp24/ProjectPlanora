using Planora.DataAccess.Models;
using Planora.DTO.Category;

namespace Planora.DataAccess.Mappers;

public static class CategoryMapping 
{
	public static CategoryDB ToEntity(CategoryDTO categoryDto) 
	{
		return new CategoryDB
		{
			CategoryId = Guid.NewGuid(),
			Name = categoryDto.Name, 
			Content = categoryDto.Content,
			HexColor =  categoryDto.HexColor
		};
	}
	
	public static CategoryDTO ToDTO(CategoryDB categoryDb) {
		return new CategoryDTO(
			CategoryId: categoryDb.CategoryId.ToString(), 
			Name: categoryDb.Name, 
			Content: categoryDb.Content,
			HexColor: categoryDb.HexColor);
	}
}