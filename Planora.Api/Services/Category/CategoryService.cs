using Planora.DataAccess.Mappers;
using Planora.DataAccess.Repositories.Category;
using Planora.DTO.CategoryDTO;
using Planora.Api.Services.Category;

namespace Planora.Api.Services.Category;

public class CategoryService : ICategoryService
{
	private readonly ICategoryRepository _categoryRepository;
	
	public CategoryService(ICategoryRepository categoryRepository) 
	{
		_categoryRepository = categoryRepository;
	}
	
	public async Task<CategoryDTO> CreateAsync(CategoryDTO dto)
	{
		var categoryDB = CategoryMapping.ToEntity(dto);
		var createdCategoryDB = await _categoryRepository.CreateAsync(categoryDB);
		return CategoryMapping.ToDTO(createdCategoryDB);
	}

	public async Task<IEnumerable<CategoryDTO>> GetAllAsync()
	{
		var categoryDbs = await _categoryRepository.GetAllAsync();
		return categoryDbs.Select(CategoryMapping.ToDTO);
	}
	
	public async Task<CategoryDTO> GetByIdAsync(string categoryId)
	{
		if (!Guid.TryParse(categoryId, out var cGuid))
		{
			throw new ArgumentException($"Invalid categoryId {categoryId}");
		}

		var categoryDB = await _categoryRepository.GetByIdAsync(cGuid);
		return CategoryMapping.ToDTO(categoryDB);
	}
	
	public async Task<CategoryDTO> UpdateAsync(string categoryId, CategoryDTO dto)
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
		categoryDB.Name = dto.Name;
		categoryDB.HexColor = dto.HexColor;
		await _categoryRepository.SaveChangesAsync();
		return CategoryMapping.ToDTO(categoryDB);
	}

	public async Task<CategoryDTO> DeleteAsync(string categoryId)
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