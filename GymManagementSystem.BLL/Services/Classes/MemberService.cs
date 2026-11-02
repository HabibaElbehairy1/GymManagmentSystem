using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagment.DAL;
using GymManagment.DAL.Models;
using GymManagment.DAL.Models.Enums;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Member> _repo;

        public MemberService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _repo = _unitOfWork.GetRepository<Member>();

        }
        //all member
        public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken ct)
        {
            var members = await _repo.GetAllAcync(ct:ct);
            var result = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return result;
        }


        //member details
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            var member = await _repo.GetByIdAcync(memberId,ct);
            if (member is null)
                return null;


            var activePlan = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);
            Plan? plan = null;
            if (activePlan is not null)
            {
                plan = await _unitOfWork.GetRepository<Plan>().FirstOrDefaultAsync(p => p.Id == activePlan.PlanId && p.IsActive == true, ct);
            }
            var result = _mapper.Map<MemberViewModel>(member);


            if (activePlan is not null && plan is not null)
            {
                result.MemberShipStartDate = activePlan.CreatedAt.ToString();
                result.MemberShipEndDate = activePlan.EndDate.ToString();
                result.PlanName = plan.Name;
            }
            return result;
        }


        //health record
        public async Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            var memberHealth = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(h => h.MemberId == memberId, ct);

            if (memberHealth is null)
                return null;

            var result = _mapper.Map<HealthRecordViewModel>(memberHealth);
            return result;
        }


        //create
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var EmailCheck = await _repo.AnyAsync(m => m.Email == model.Email ,ct);
            var PhoneCheck = await _repo.AnyAsync(m => m.Phone == model.Phone ,ct);

            if(EmailCheck || PhoneCheck) 
                return false;
            var member = _mapper.Map<Member>(model);
            _repo.Add(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }


        //update
        public async Task<UpdateMemberViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var member = await _repo.GetByIdAcync(memberId);
            if (member is null)
                return null;

            var data = _mapper.Map<UpdateMemberViewModel>(member);
            return data;
        }



      
        public async Task<bool> UpdateMemberAsync(int memberId, UpdateMemberViewModel model, CancellationToken ct)
        {
            var member = await _repo.GetByIdAcync(memberId, ct);

            var EmailCheck = await _repo.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var PhoneCheck = await _repo.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);

            if (EmailCheck || PhoneCheck) return false;
            member.Name = model.Name;
            member.Email=model.Email;
            member.Phone=model.Phone;
            member.Photo=model.Photo;
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;
            member.DateOfBirth = model.DateOfBirth;
            member.LastUpdatedAt = DateTime.UtcNow;

            _repo.Update(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        
        }

            //delete
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            var member = await _repo.GetByIdAcync(memberId);
            if (member is null) return false;
            _repo.Delete(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

    }
}
