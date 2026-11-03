
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System.Linq.Expressions;

namespace GymManagement.DAL.Repositories.Interfaces

{
    public interface IMembershipRepository : IGenericRepository<MemberShip>
	{
        Task<List<MemberShip>> GetAllMembershipsWithMemberAndPlanAsync(Expression<Func<MemberShip, bool>>? predicate = null,CancellationToken ct = default);
    }
}
