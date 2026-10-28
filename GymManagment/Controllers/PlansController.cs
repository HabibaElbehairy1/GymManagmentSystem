using GymManagment.DAL.Repositories.Classes;
using GymManagment.DAL.Repositories.Interfaces;
using GymManagment.DbContexts;
using GymManagment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagment.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct = default)
        {
            var plans = await _planRepository.GetAllAcync(ct:ct);
            return View(plans);
        }
        public async Task<IActionResult> Details(int id , CancellationToken ct = default)
        {
            var plan = await _planRepository.GetByIdAcync(id, ct);
            if (plan is null) return RedirectToAction(nameof(Index));
            return View(plan);
        }
    }
}
