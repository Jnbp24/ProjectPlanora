using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.Category;

public class CategoryRepository : Repository<CategoryDB>, ICategoryRepository
{
	public CategoryRepository(DatabaseContext context) : base(context)
	{
	}
}