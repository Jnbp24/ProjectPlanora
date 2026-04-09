using Planora.DTO.CategoryDTO;

namespace Planora.DataAccess.Repositories.Category;

public interface ICategoryRepository
{
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync();
}