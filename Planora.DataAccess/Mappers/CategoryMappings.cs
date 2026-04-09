using Planora.DataAccess.Models;
using Planora.DTO.CategoryDTO;

namespace Planora.DataAccess.Mappers;

public class CategoryMappings {
	public static CategoryDTO MapCategoryDBtoCategoryDTO(CategoryDB categoryDb) {
		return new CategoryDTO(categoryDb.Id, categoryDb.Name, categoryDb.HexColor);
	}
	
	public static CategoryDB MapCategoryDTOtoCategoryDB(CategoryDTO categoryDto) {
		return new CategoryDB(categoryDto.Name, categoryDto.HexColor);
	}
	
	public static List<CategoryDTO> MapCategoriesDBtoCategoriesDTO(List<CategoryDB> categoriesDbs) {
		List<CategoryDTO> categoriesDtos = [];
		foreach (var categoryDb in categoriesDbs) {
			categoriesDtos.Add(MapCategoryDBtoCategoryDTO(categoryDb));
		}

		return categoriesDtos;
	}
	
}