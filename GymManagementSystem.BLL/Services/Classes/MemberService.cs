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
        private readonly IUnitOfWork _unitOFWork;

        public MemberService(IUnitOfWork unitOFWork)
        {
            _unitOFWork = unitOFWork;
        }
        //all member
        public async Task<IEnumerable<MemberViewModel>> GetMembersAsync(CancellationToken ct)
        {
            var members = await _unitOFWork.GetRepository<Member>().GetAllAcync(ct:ct);
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
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAcync(memberId,ct);
            if (member is null)
                return null;


            var activePlan = await _unitOFWork.GetRepository<MemberShip>().FirstOrDefaultAsync(m => m.MemberId == memberId && m.EndDate > DateTime.Now, ct);
            Plan? plan = null;
            if (activePlan is not null)
            {
                plan = await _unitOFWork.GetRepository<Plan>().FirstOrDefaultAsync(p => p.Id == activePlan.PlanId && p.IsActive == true, ct);
            }
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
            var memberHealth = await _unitOFWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(h => h.MemberId == memberId, ct);

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
            var EmailCheck = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email ,ct);
            var PhoneCheck = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone ,ct);

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
             _unitOFWork.GetRepository<Member>().Add(member);
            var result = await _unitOFWork.SaveChangesAsync(ct);
            return result > 0;

        }


        //update
        public async Task<UpdateMemberViewModel> GetMemberToUpdateAsync(int memberId, CancellationToken ct)
        {
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAcync(memberId);
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
            var member = await _unitOFWork.GetRepository<Member>().GetByIdAcync(memberId, ct);

            var EmailCheck = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var PhoneCheck = await _unitOFWork.GetRepository<Member>().AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);

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

             _unitOFWork.GetRepository<Member>().Update(member);
            var result = await _unitOFWork.SaveChangesAsync(ct);
            return result > 0;
        
        }

            //delete
        public async Task<bool> DeleteMemberAsync(int memberId, CancellationToken ct)
        {
            var member =await _unitOFWork.GetRepository<Member>().GetByIdAcync(memberId);
            if (member is null) return false;
            _unitOFWork.GetRepository<Member>().Delete(member);
            var result = await _unitOFWork.SaveChangesAsync(ct);
            return result > 0;

        }

    }
}
