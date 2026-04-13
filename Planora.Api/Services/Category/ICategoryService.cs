using System.Collections.Generic;
using System.Threading.Tasks;
using Planora.DTO.CategoryDTO;

namespace Planora.Api.Services.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDTO>> GetAllAsync();
    }
}
