using Planora.DTO.CategoryDTO;

namespace Planora.Api.Services.Category;

public interface ICategoryService
{
    Task<CategoryDTO> CreateCategoryAsync(CategoryDTO categoryDto);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
    Task<CategoryDTO> GetCategoryByIdAsync(string categoryId);
    Task<CategoryDTO> UpdateCategoryAsync(string categoryId, CategoryDTO categoryDto);
    Task<CategoryDTO> DeleteCategoryAsync(string categoryId);
}