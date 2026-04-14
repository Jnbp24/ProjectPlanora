using System.Collections.Generic;
using System.Threading.Tasks;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Services.Category
{
    public interface ICategoryService
    {
        Task<CategoryDTO> CreateAsync(CategoryDTO categoryDto);
        Task<CategoryDTO> UpdateAsync(string categoryId, CategoryDTO categoryDto);
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
        Task<CategoryDTO?> GetByIdAsync(string categoryId);
        Task<CategoryDTO> DeleteAsync(string categoryId);
    }
}
