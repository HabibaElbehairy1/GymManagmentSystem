using GymManagementSystem.BLL.Services.Interfaces;
using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Plans;
using GymManagment.DbContexts;
using GymManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanService _planService;

        public PlansController(IPlanService planService)
        {

            _planService = planService;
        }
        //all
        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _planService.GetPlansAsync(ct: ct);
            return View(plans);
        }

        //details
        public async Task<IActionResult> Details(int id, CancellationToken ct = default)
        {
            var plan = await _planService.GetplanDetailsAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = " Member Not Found !";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        //activation
        [HttpPost]
        public async Task<IActionResult> ToggleActivation(int id, CancellationToken ct)
        {
            bool activation = await _planService.ToggleActivationAsync(id, ct);
            TempData["IsToggeled"] = activation;
            return RedirectToAction(nameof(Index));

        }

        //update
        [HttpGet]
        public async Task<IActionResult> Update(int id, CancellationToken ct)
        {
            var plan = await _planService.ViewPlanToUpdateAsync(id, ct);
            if (plan is null)
            {
                TempData["ErrorMessage"] = " Member Not Found !";
                return null;

            }
            return View(plan);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdatePlanModelView model, CancellationToken ct)
        {
            bool plan = await _planService.UpdatePlanAsync(id, model, ct);
            TempData["IsUpdated"] = plan;

            return RedirectToAction(nameof(Index));

        }

    }
}
