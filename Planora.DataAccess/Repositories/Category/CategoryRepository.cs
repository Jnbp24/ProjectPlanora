using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Mappers;
using Planora.DTO.CategoryDTO;

namespace Planora.DataAccess.Repositories.Category;

public class CategoryRepository {
	private DatabaseContext _context;

	public CategoryRepository(DatabaseContext context) {
		_context = context;
	}

	public async Task<List<CategoryDTO>> GetCategoriesAsync() {
		return CategoryMappings.MapCategoriesDBtoCategoriesDTO(await _context.Categories.ToListAsync());
	}
}