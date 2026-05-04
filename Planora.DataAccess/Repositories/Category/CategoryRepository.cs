using Microsoft.EntityFrameworkCore;
using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Category;

public class CategoryRepository : Repository<CategoryDB>, ICategoryRepository
{
	public CategoryRepository(DatabaseContext context) : base(context)
	{
	}
	public override async Task<IEnumerable<CategoryDB>> GetAllAsync()
	{
		return await _dbContext.Categories.Where(c => !c.Deleted).Include(c => c.Tasks).ToListAsync();
	}

	public override async Task<CategoryDB> GetByIdAsync(Guid categoryId)
	{
		return await _dbContext.Categories.Where(c => !c.Deleted).FirstOrDefaultAsync(c => c.CategoryId == categoryId) ?? throw new KeyNotFoundException($"Key {categoryId} does not exist.");
	}

}