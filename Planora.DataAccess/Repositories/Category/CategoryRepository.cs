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

	public override async Task<CategoryDB> GetByIdAsync(string id)
	{
		return await _dbContext.Categories.Where(c => !c.Deleted).FirstOrDefaultAsync(c => c.CategoryId == Guid.Parse(id)) ?? throw new KeyNotFoundException($"Key {id} does not exist.");
	}

}