using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IPlanService
    {
        Task<IEnumerable<PlanViewModel>> GetPlansAsync(CancellationToken ct);
        Task<PlanViewModel?> GetplanDetailsAsync(int planId, CancellationToken ct);
        Task<bool> ToggleActivationAsync(int planId, CancellationToken ct);
        Task<UpdatePlanModelView> ViewPlanToUpdateAsync(int planId, CancellationToken ct);
        Task<bool> UpdatePlanAsync(int planId, UpdatePlanModelView model, CancellationToken ct);

    }
}
