using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Sessions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagment.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        private async Task LoadDropdownsAsync(CancellationToken ct)
        {
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesAsync(ct), "Id", "Name");
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersAsync(ct), "Id", "Name");
        }
        //index
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionService.GetSessionsAsync(ct);
            return View(sessions);
        }
        //session
        public async Task<IActionResult?> Details(int Id, CancellationToken ct)
        {
            var session = await _sessionService.GetSessionDetailsAsync(Id, ct);
            if (session is null)
            {
                TempData["ErrorMessage"] = " Session Not Found !";

                return RedirectToAction("Index");
            }
            return View(session);
        }


        //create
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await LoadDropdownsAsync(ct);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _sessionService.CreateSessionAsync(model, ct);
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
        public async Task<IActionResult> Update(int id, CancellationToken ct)
        {
            await LoadDropdownsAsync(ct);
            var data = await _sessionService.GetSessionToUpdateAsync(id, ct);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateSessionViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _sessionService.UpdateSessionAsync(id, model, ct);

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
            bool result = await _sessionService.DeleteSessionAsync(id, ct);
            TempData["IsDeleted"] = result;
            return RedirectToAction("Index");
        }

    }
}
