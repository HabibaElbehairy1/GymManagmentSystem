
using GymManagement.BLL.ViewModels.MemberShip;
using GymManagementSystem.BLL.Common;

namespace GymManagementSystem.BLL.Services.Classes
{
	public interface IMembershipService
	{
        Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default);
        Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default);
        Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default);
        Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default);
        Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken ct = default);

    }
}
