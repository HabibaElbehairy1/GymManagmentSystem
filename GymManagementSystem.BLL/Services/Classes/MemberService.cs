using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
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
        private readonly IGenericRepository<Member> _memberRepository;
        private readonly IGenericRepository<MemberShip> _membershipRepository;
        private readonly IGenericRepository<Plan> _memberPlanRepository;
        private readonly IGenericRepository<HealthRecord> _memberHealthRepository;

        public MemberService
            (IGenericRepository<Member> memberRepository,
             IGenericRepository<MemberShip> membershipRepository,
             IGenericRepository<Plan> memberPlanRepository,
             IGenericRepository<HealthRecord> memberHealthRepository)
        {
            _memberHealthRepository = memberHealthRepository;
            _memberRepository = memberRepository;
            _membershipRepository = membershipRepository;
            _memberPlanRepository = memberPlanRepository;
        }
        //all member
        public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken ct)
        {
            var members = await _memberRepository.GetAllAcync(ct:ct);
            var membersViewModels = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Phone = member.Phone,
                Name = member.Name,
                Email = member.Email,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
            });

            return membersViewModels;
        }


        //member details
        public async Task<MemberViewModel?> GetMemberDetailsAsync(int memberId, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAcync(memberId,ct);
            var activePlan = await _membershipRepository.FirstOrDefaultAsync( m => m.MemberId == memberId && m.EndDate>DateTime.Now , ct);
            var plan = await _memberPlanRepository.FirstOrDefaultAsync(p => p.Id == activePlan.PlanId && p.IsActive ==true, ct);
            if (member is null)
                return null;
            var model = new MemberViewModel()
            {
                Id = member.Id,
                Phone = member.Phone,
                Name = member.Name,
                Email = member.Email,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToString(),
                Address = $"{member.Address.BuildingNumber } - {member.Address.Street} Street - {member.Address.City}",
 
            };
            if (activePlan is not null && plan is not null) 
            {
                model.MemberShipStartDate = activePlan.CreatedAt.ToString();
                model.MemberShipEndDate = activePlan.EndDate.ToString();
                model.PlanName = plan.Name;
            }

            return model;
        }


        //health record
        public async Task<HealthRecordViewModel> GetMemberHealthRecordAsync(int memberId, CancellationToken ct)
        {
            var memberHealth = await _memberHealthRepository.FirstOrDefaultAsync(h => h.MemberId == memberId, ct);

            if (memberHealth is null)
                return null;

            return new HealthRecordViewModel()
            {
                Weight = memberHealth.Weight,
                Height = memberHealth.Height,
                BloodType = memberHealth.BloodType,
                Note = memberHealth.Note,
            };
        }


        //create
        public async Task<bool> CreateMemberAsync(CreateMemberViewModel model, CancellationToken ct)
        {
            var EmailCheck = await _memberRepository.AnyAsync(m => m.Email == model.Email ,ct);
            var PhoneCheck = await _memberRepository.AnyAsync(m => m.Phone == model.Phone ,ct);

            if(EmailCheck || PhoneCheck) 
                return false;
            var member = new Member()
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    City = model.City,
                    Street = model.Street,
                },
                HealthRecord = new HealthRecord()
                {
                    BloodType = model.HealthRecordViewModel.BloodType,
                    Weight = model.HealthRecordViewModel.Weight,
                    Height = model.HealthRecordViewModel.Height,
                    Note = model.HealthRecordViewModel.Note,
                },
            };
            var createMember = await _memberRepository.AddAcync(member, ct);
            return createMember > 0;

        }


        //update
        public async Task<UpdateMemberViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAcync(memberId);
            if (member is null)
                return null;
            var data = new UpdateMemberViewModel()
            {
                Photo = member.Photo,
                Phone = member.Phone,
                Name = member.Name,
                Email = member.Email,
                BuildingNumber = member.Address.BuildingNumber,
                Street=member.Address.Street,
                City=member.Address.City,


            };
            return data;
        }



      
        public async Task<bool> UpdateMemberAsync(int memberId, UpdateMemberViewModel model, CancellationToken ct)
        {
            var member = await _memberRepository.GetByIdAcync(memberId, ct);

            var EmailCheck = await _memberRepository.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var PhoneCheck = await _memberRepository.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);

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

            var updateMember = await _memberRepository.UpdateAcync(member, ct);
            return updateMember > 0;
        }

            //delete
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            var member =await _memberRepository.GetByIdAcync(memberId);
            if (member is null) return false;
            var deleteM = await _memberRepository.DeleteAcync(member, ct);
            return deleteM > 0;
        }

    }
}
