using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Category;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Services.Category;

public class CategoryService : ICategoryService
{
	private readonly ICategoryRepository _categoryRepository;
	
	public CategoryService(ICategoryRepository categoryRepository) 
	{
		_categoryRepository = categoryRepository;
	}
	
	public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDTO)
	{
		var categoryDB = CategoryMapping.ToEntity(categoryDTO);
		var createdCategoryDB = await _categoryRepository.CreateAsync(categoryDB);
		return CategoryMapping.ToDTO(createdCategoryDB);
	}

	public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
	{
		var categoryDbs = await _categoryRepository.GetAllAsync();
		return categoryDbs.Select(CategoryMapping.ToDTO);
	}
	
	public async Task<CategoryDTO> GetCategoryByIdAsync(string categoryId)
	{
		if (!Guid.TryParse(categoryId, out var cGuid))
		{
			throw new ArgumentException($"Invalid categoryId {categoryId}");
		}

		var categoryDB = await _categoryRepository.GetByIdAsync(cGuid);
		return CategoryMapping.ToDTO(categoryDB);
	}
	
	public async Task<CategoryDTO> UpdateCategoryByIdAsync(string categoryId, CategoryDTO categoryDTO)
	{
		if (!Guid.TryParse(categoryId, out var cGuid))
		{
			throw new ArgumentException($"Invalid categoryId {categoryId}");
		}
		
		var categoryDB = await _categoryRepository.GetByIdAsync(cGuid);
		if (categoryDB.Deleted)
		{
			throw new NotSupportedException($"{categoryId} is already deleted");
		}
		categoryDB.Name = categoryDTO.Name;
		categoryDB.Content = categoryDTO.Content;
		categoryDB.HexColor = categoryDTO.HexColor;
		await _categoryRepository.SaveChangesAsync();
		return CategoryMapping.ToDTO(categoryDB);
	}

	public async Task<CategoryDTO> DeleteCategoryByIdAsync(string categoryId)
	{
		if (!Guid.TryParse(categoryId, out var cGuid))
		{
			throw new ArgumentException($"Invalid categoryId {categoryId}");
		}
		
		var categoryDB = await _categoryRepository.GetByIdAsync(cGuid);
		if (categoryDB.Deleted)
		{
			throw new NotSupportedException($"{categoryId} is already deleted");
		}
		categoryDB.Deleted = true;
		await _categoryRepository.SaveChangesAsync();
		return CategoryMapping.ToDTO(categoryDB);
	}

}