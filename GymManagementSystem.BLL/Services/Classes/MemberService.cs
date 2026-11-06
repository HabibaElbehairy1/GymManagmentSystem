using AutoMapper;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagement.DAL;
using GymManagement.DAL.Models;
using GymManagement.DAL.Models.Enums;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using GymManagementSystem.BLL.Services.Attachment;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;
        private readonly IGenericRepository<Member> _repo;

        public MemberService(IUnitOfWork unitOfWork,
                             IMapper mapper,
                             IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
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
            var fileName = await _attachmentService.UploadAsync(model.PhotoFile.OpenReadStream() , "MembersPicture" , model.PhotoFile.FileName , ct);
            if(string.IsNullOrWhiteSpace(fileName)) return false;

            var member = _mapper.Map<Member>(model);
            member.Photo = fileName;
            _repo.Add(member);

            var result = await _unitOfWork.SaveChangesAsync(ct);
            if (result > 0)
                return true;
            else
            {
                _attachmentService.Delete("MembersPicture",fileName);
                return false;
            }

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
            if (member is null) return false;

            var emailCheck = await _repo.AnyAsync(m => m.Email == model.Email && m.Id != memberId, ct);
            var phoneCheck = await _repo.AnyAsync(m => m.Phone == model.Phone && m.Id != memberId, ct);
            if (emailCheck || phoneCheck) return false;

            string? newFileName = null;
            var oldFileName = member.Photo;

            if (model.PhotoFile is not null && model.PhotoFile.Length > 0)
            {
                newFileName = await _attachmentService.UploadAsync(
                    model.PhotoFile.OpenReadStream(), "MembersPicture", model.PhotoFile.FileName, ct);

                if (string.IsNullOrWhiteSpace(newFileName)) return false;

                member.Photo = newFileName;
            }

            member.Name = model.Name;
            member.Email = model.Email;
            member.Phone = model.Phone;
            member.DateOfBirth = model.DateOfBirth;
            member.LastUpdatedAt = DateTime.UtcNow;

            member.Address ??= new Address();
            member.Address.BuildingNumber = model.BuildingNumber;
            member.Address.Street = model.Street;
            member.Address.City = model.City;

            _repo.Update(member);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result > 0)
            {
                if (newFileName is not null && !string.IsNullOrWhiteSpace(oldFileName))
                    _attachmentService.Delete("MembersPicture", oldFileName);
                return true;
            }

            if (newFileName is not null)
                _attachmentService.Delete("MembersPicture", newFileName);

            return false;
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
