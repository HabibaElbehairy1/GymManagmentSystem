using AutoMapper;
using GymManagementSystem.BLL.ViewModels.MemberShip;
using GymManagementSystem.BLL.Common;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using Microsoft.Extensions.Logging;

namespace GymManagementSystem.BLL.Services.Classes
{
	public class MembershipService : IMembershipService
	{
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<MemberShip> _repo;

        public MembershipService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo =_unitOfWork.GetRepository<MemberShip>();
        }

        public async Task<Result> CreateMembershipAsync(CreateMemberShipViewModel model, CancellationToken ct = default)
        {
            var memberExists = await _unitOfWork.GetRepository<Member>().AnyAsync(m => m.Id == model.MemberId, ct);
            if (!memberExists) return Result.NotFound("Member not found.");

            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAcync(model.PlanId, ct);
            if (plan is null) return Result.NotFound("Plan not found.");
            if (!plan.IsActive) return Result.Fail("Plan is not active." ,ResultKind.Conflict);


            var hasActive = await _repo.AnyAsync(m => m.MemberId == model.MemberId && m.EndDate > DateTime.Now, ct);
            if (hasActive) return Result.Fail("Member already has an active membership.",ResultKind.Conflict);

            var entity = new MemberShip
            {
                MemberId = model.MemberId,
                PlanId = plan.Id,
                CreatedAt = DateTime.Now,
                EndDate = (model.StartDate ?? DateTime.Now).AddDays(plan.DurationDays),
            };

            _repo.Add(entity);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Create New Membership", ResultKind.Conflict);
        }

        public async Task<Result> DeleteActiveMembershipAsync(int memberId, CancellationToken ct = default)
        {
            var active = await _repo.FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct: ct);

            if (active is null) return Result.NotFound("No active membership for this member.");

            _repo.Delete(active);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Delete Membership", ResultKind.Conflict);
        }

        public async Task<IEnumerable<MemberShipViewModel>> GetAllMembershipsAsync(CancellationToken ct = default)
        {
            var memberships = await _unitOfWork.MembershipRepository.GetAllMembershipsWithMemberAndPlanAsync(m => m.EndDate > DateTime.Now, ct);
            return _mapper.Map<IEnumerable<MemberShipViewModel>>(memberships);
        }

        public async Task<IEnumerable<PlanSelectListViewModel>> GetPlansForDropDownAsync(CancellationToken ct = default)
        {
            var plans = await _unitOfWork.GetRepository<Plan>().GetAllAcync(ct: ct);
            var activePlans = plans.Where(p => p.IsActive);

            return _mapper.Map<IEnumerable<PlanSelectListViewModel>>(activePlans);
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropDownAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAcync(ct: ct);
            return _mapper.Map<IEnumerable<MemberSelectListViewModel>>(members);
        }


    }
}
