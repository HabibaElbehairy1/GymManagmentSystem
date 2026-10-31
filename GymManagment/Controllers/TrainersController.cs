using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.Members;
using GymManagementSystem.BLL.ViewModels.Trainers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GymManagment.PL.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService) 
        {
            _trainerService = trainerService;
        }
        //index
        public async Task<IActionResult> Index(CancellationToken ct) 
        {
            var trainers = await _trainerService.GetTrainersAsync(ct:ct);
            return View(trainers);
        }
        public async Task<IActionResult?> Details(int Id, CancellationToken ct)
        {
            var trainer = await _trainerService.TrainerDetailsAsync(Id, ct);
            if (trainer is null)
            {
                return RedirectToAction("Index");
                TempData["ErrorMessage"] = " Trainer Not Found !";
            }
            return View(trainer);
        }
        //create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _trainerService.CreateTrainerAsync(model, ct);
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
            var data = await _trainerService.GetTrainerToUpdateAsync(id, ct);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateTrainerViewModel model, CancellationToken ct)
        {
            if (ModelState.IsValid)
            {
                bool result = await _trainerService.UpdateTrainerAsync(id, model, ct);

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
            bool result = await _trainerService.DeleteTrainerAsync(id, ct);
            TempData["IsDeleted"] = result;
            return RedirectToAction("Index");
        }


    }
}
