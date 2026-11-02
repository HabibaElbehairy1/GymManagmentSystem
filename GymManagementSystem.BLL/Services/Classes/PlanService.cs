using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagment.DAL;
using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.Models;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Numerics;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Plan> _repo;

        public PlanService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Plan>();
        }

        //all
        public async Task<IEnumerable<PlanViewModel>> GetPlansAsync(CancellationToken ct)
        {
            var plans = await _repo.GetAllAcync(ct:ct);
            var getPlans = _mapper.Map<IEnumerable<PlanViewModel>>(plans);

            return getPlans;
        }


        //Plan details
        public async Task<PlanViewModel?> GetplanDetailsAsync(int planId, CancellationToken ct)
        {
            var plan = await _repo.GetByIdAcync(planId,ct);
            if (plan is null)
                return null;
            var getPlan = _mapper.Map<PlanViewModel>(plan);
            return getPlan;
        }


        //activation
        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct)
        {
            var plan = await _repo.GetByIdAcync(planId, ct);
            if (plan is null)
                return false;
            plan.IsActive = !plan.IsActive;
            plan.LastUpdatedAt = DateTime.UtcNow;

            _repo.Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }


        //update
        public async Task<UpdatePlanModelView> ViewPlanToUpdateAsync(int planId, CancellationToken ct)
        {
            var plan = await _repo.GetByIdAcync(planId, ct);
            if (plan is null)
                return null;
            var updatePlan = _mapper.Map<UpdatePlanModelView>(plan);
            return updatePlan;
        }

        public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanModelView model, CancellationToken ct)
        {
            var plan = await _repo.GetByIdAcync(planId, ct);

            _mapper.Map(model, plan);
            plan.LastUpdatedAt = DateTime.UtcNow;

            _repo.Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }


    }
}
