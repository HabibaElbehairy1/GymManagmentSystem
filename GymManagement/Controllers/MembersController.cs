using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
using GymManagement.Models;
using GymManagementSystem.BLL.Services.Attachment;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.PL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]


    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberService ,
                                  IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }

        public async Task<IActionResult> Picture(int id , CancellationToken ct=default)
        {
            var member = await _memberService.GetMemberDetailsAsync(id , ct);
            if (member == null|| string.IsNullOrWhiteSpace(member.Photo)) return NotFound();
            var photo =  _attachmentService.GetFile("MembersPicture",member.Photo);
            if (photo == null) return NotFound();
            return File(
                photo.Value.stream,
                photo.Value.contentType);
        }

        //index
        public async Task<IActionResult> Index(CancellationToken ct) 
        {
            var members =await _memberService.GetMembersAsync(ct);
            return View(members);
        }
        //member
        public async Task<IActionResult?> MemberDetails(int Id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsAsync(Id, ct);
            if (member is null) 
            {
                TempData["ErrorMessage"] = " Member Not Found !";
                return RedirectToAction("Index");
            }
            return View(member);
        }

        //healthrecord
        public async Task<IActionResult?> MemberHealthRecord(int Id, CancellationToken ct)
        {
            var memberHealth = await _memberService.GetMemberHealthRecordAsync(Id, ct);
            if (memberHealth is null) return RedirectToAction(nameof(Index));
            return View(memberHealth);
        }

        //create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberViewModel model,CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _memberService.CreateMemberAsync(model, ct);
                TempData["IsCreated"] = result;
                return RedirectToAction("Index");

            }
            else 
            {
                return View(model);

            }

        }

        //update
        [HttpGet]
        public async Task<IActionResult> Update(int id,CancellationToken ct)
        {
            var data = await _memberService.GetMemberToUpdateAsync(id ,ct);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateMemberViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _memberService.UpdateMemberAsync(id, model, ct);

                TempData["IsUpdated"] = result;
            }
            return RedirectToAction("Index");

        }

        //delete
        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            bool result = await _memberService.DeleteMemberAsync(id, ct);
            TempData["IsDeleted"] = result;
            return RedirectToAction("Index");
        }
    }
}
