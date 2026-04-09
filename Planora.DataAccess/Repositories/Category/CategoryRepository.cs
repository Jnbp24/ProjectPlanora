using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Mappers;
using Planora.DTO.CategoryDTO;

namespace Planora.DataAccess.Repositories.Category;

public class CategoryRepository : ICategoryRepository
{
	private DatabaseContext _context;

	public CategoryRepository(DatabaseContext context) 
	{
		_context = context;
	}

	public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync()
	{
		var categories = await _context.Categories.ToListAsync();
		return categories.Select(CategoryMapping.ToDTO);
	}
}