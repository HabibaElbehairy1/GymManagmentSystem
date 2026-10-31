using GymManagementSystem.BLL.ViewModels.Trainers;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetTrainersAsync(CancellationToken ct);
        Task<TrainerViewModel> TrainerDetailsAsync(int trainerId,CancellationToken ct);
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel model, CancellationToken ct);
        Task<UpdateTrainerViewModel> GetTrainerToUpdateAsync( int trainerId, CancellationToken ct);
        Task<bool> UpdateTrainerAsync(int trainerId , UpdateTrainerViewModel model, CancellationToken ct);
        Task<bool> DeleteTrainerAsync(int trainerId,CancellationToken ct);

    }
}
