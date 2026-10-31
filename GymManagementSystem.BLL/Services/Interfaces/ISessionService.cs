using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.BLL.ViewModels.Sessions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken ct);
        Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct);
        Task<bool> CreateSessionAsync(SessionViewModel model, CancellationToken ct);
        Task<SessionViewModel> ViewSessionToUpdateAsync(int sessionId, CancellationToken ct);
        Task<bool> UpdateSessionAsync(int sessionId, SessionViewModel model, CancellationToken ct);
        Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct);

    }
}
