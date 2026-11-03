using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Home;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeViewModel> GetHomeStatsAsync(CancellationToken ct)
        {
            var now = DateTime.Now;

            var members = _unitOfWork.GetRepository<Member>();
            var trainers = _unitOfWork.GetRepository<Trainer>();
            var memberships = _unitOfWork.GetRepository<MemberShip>();
            var sessions = _unitOfWork.GetRepository<Session>();   

            return new HomeViewModel
            {
                TotalMembers = await members.CountAsync(ct: ct),
                TotalTrainers = await trainers.CountAsync(ct: ct),
                ActiveMembers = await memberships.CountAsync(m => m.EndDate > now, ct),
                UpcomingSessions = await sessions.CountAsync(s => s.StartDate > now, ct),
                OngoingSessions = await sessions.CountAsync(s => s.StartDate <= now && s.EndDate >= now, ct),
                CompletedSessions = await sessions.CountAsync(s => s.EndDate < now, ct),
            };
        }

    }
}
