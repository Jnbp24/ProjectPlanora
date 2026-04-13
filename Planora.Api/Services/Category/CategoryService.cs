using Planora.DataAccess.Repositories.Category;
using Planora.DTO.CategoryDTO;
using Planora.Api.Services.Category;

namespace Planora.Api.Services.Category;

public class CategoryService : ICategoryService
{
	private ICategoryRepository _categoryRepository;
	
	public CategoryService(ICategoryRepository categoryRepository) 
	{
		_categoryRepository = categoryRepository;
	}

	public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
	{
		return await _categoryRepository.GetAllCategoriesAsync();
	}

}