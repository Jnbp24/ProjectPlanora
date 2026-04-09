using Planora.DataAccess.Repositories.Category;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Services;

public class CategoryService {
	private CategoryRepository _categoryRepository;
	public CategoryService(CategoryRepository categoryRepository) {
		_categoryRepository = categoryRepository;
	}

	public async Task<List<CategoryDTO>> GetCategoriesAsync() {
		return await _categoryRepository.GetCategoriesAsync();
	}
}