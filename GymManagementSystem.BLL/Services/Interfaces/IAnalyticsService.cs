using GymManagementSystem.BLL.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        Task<HomeViewModel> GetHomeStatsAsync(CancellationToken ct = default);
    }
}
