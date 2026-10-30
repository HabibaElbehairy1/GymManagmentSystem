using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagment.DAL.Models;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;

        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
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
                return RedirectToAction("Index");
                TempData["ErrorMessage"] = " Member Not Found !";
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
