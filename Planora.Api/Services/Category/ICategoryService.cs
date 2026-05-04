using Planora.DTO.Category;

namespace Planora.Api.Services.Category;

public interface ICategoryService
{
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(string categoryId);
    Task<CategoryDTO> UpdateCategoryByIdAsync(string categoryId, CategoryDTO categoryDto);
    Task<CategoryDTO> DeleteCategoryByIdAsync(string categoryId);
}