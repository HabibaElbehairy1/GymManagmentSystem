using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagementSystem.BLL.ViewModels.Sessions;
using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ISessionService
    {
        Task<IEnumerable<SessionViewModel>> GetSessionsAsync(CancellationToken ct);
        Task<SessionViewModel?> GetSessionDetailsAsync(int sessionId, CancellationToken ct);
        Task<bool> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct);
        Task<IEnumerable<Category>> GetCategoriesAsync(CancellationToken ct);
        Task<IEnumerable<Trainer>> GetTrainersAsync(CancellationToken ct);


        Task<UpdateSessionViewModel> GetSessionToUpdateAsync(int sessionId, CancellationToken ct);
        Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionViewModel model, CancellationToken ct);
        Task<bool> DeleteSessionAsync(int sessionId, CancellationToken ct);

    }
}
