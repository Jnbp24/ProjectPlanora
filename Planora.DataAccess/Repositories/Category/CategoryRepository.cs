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
		return await _dbContext.Categories.Include(c => c.Tasks).ToListAsync();
	} 
}