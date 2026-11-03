
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace GymManagement.DAL.Repositories.Classes
{
	public class MembershipRepository : GenericRepository<MemberShip>, IMembershipRepository
	{
		private readonly GymDbContext _context;

		public MembershipRepository(GymDbContext dbContext) : base(dbContext)
		{
			_context = dbContext;
		}

        public async Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null,
           CancellationToken ct = default)
        {
            IQueryable<MemberShip> query = _context.MemberShips.AsNoTracking().Include(m => m.Plan).Include(m => m.Member);

            if (predicate is not null) query = query.Where(predicate);

            return await query.ToListAsync(ct);
        }


    }
}
