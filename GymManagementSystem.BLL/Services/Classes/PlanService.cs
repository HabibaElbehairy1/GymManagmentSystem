using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
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
        private readonly IGenericRepository<Plan> _planRepository;

        public PlanService(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        //all
        public async Task<IEnumerable<PlanViewModel>> GetPlansAsync(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAcync(ct:ct);
            var getPlans = plans.Select(plan => new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
            });

            return getPlans;
        }


        //Plan details
        public async Task<PlanViewModel?> GetplanDetailsAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAcync(planId,ct);
            if (plan is null)
                return null;
            var getPlan = new PlanViewModel()
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive,
            };
            return getPlan;
        }


        //activation
        public async Task<bool> ToggleActivationAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAcync(planId, ct);
            if (plan is null)
                return false;
            plan.IsActive = !plan.IsActive;
            plan.LastUpdatedAt = DateTime.UtcNow;
            
            var result = await _planRepository.UpdateAcync(plan);
            return result > 0;

        }


        //update
        public async Task<UpdatePlanModelView> ViewPlanToUpdateAsync(int planId, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAcync(planId, ct);
            if (plan is null)
                return null;
            var updatePlan = new UpdatePlanModelView()
            {
                PlanName = plan.Name,
                Description = plan.Description,
                DurationDays = plan.DurationDays,
                Price= plan.Price,
            };
            return updatePlan;
        }

        public async Task<bool> UpdatePlanAsync(int planId, UpdatePlanModelView model, CancellationToken ct)
        {
            var plan =await _planRepository.GetByIdAcync(planId, ct);

            plan.Description = model.Description;
            plan.DurationDays = model.DurationDays;
            plan.Price = model.Price;

            var updated = await _planRepository.UpdateAcync(plan, ct);
            return updated > 0;

        }


    }
}
