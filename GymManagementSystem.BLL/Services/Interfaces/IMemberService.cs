using GymManagementSystem.BLL.ViewModels.Members;
using GymManagment.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken ct);
        Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct);
        Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken ct);
        Task<bool> CreateMemberAsync(CreateMemberViewModel model,CancellationToken ct);
        Task<UpdateMemberViewModel> GetMemberToUpdateAsync(int memberId, UpdateMemberViewModel model, CancellationToken ct);
        Task<bool> UpdateMemberAsync(int memberId,UpdateMemberViewModel model, CancellationToken ct);
        Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct);



    }
}
