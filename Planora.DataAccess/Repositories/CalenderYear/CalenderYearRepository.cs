using Planora.DataAccess.Context;
using Planora.DataAccess.Models;

namespace Planora.DataAccess.Repositories.CalenderYear;

public class CalenderYearRepository : Repository<CalenderYearDB>, ICalenderYearRepository
{
    public CalenderYearRepository(DatabaseContext context) : base(context)
    {
        
    }
}