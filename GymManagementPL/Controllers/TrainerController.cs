using GymManagementBLL.Services.CLasses;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles ="SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);
        }

        #region Get Trainer Details

        public ActionResult TrainerDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer can't be less than 1";
                return RedirectToAction(nameof(Index));
            }

            var trainer = _trainerService.GetTrainerById(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        #endregion

        #region Create Trainer

        public ActionResult CreateTrainer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTrainer(CreateTrainerViewModel createdTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Provided data is invalid");
                return View(nameof(CreateTrainer), createdTrainer);
            }
            bool isCreated = _trainerService.CreateTrainer(createdTrainer);
            if (isCreated)
            {
                TempData["SuccessMessage"] = "Trainer created successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Edit Trainer

        public ActionResult TrainerEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer can't be less than 1";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.UpdateTrainerDetails(id);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public ActionResult TrainerEdit([FromRoute]int id, TrainerToUpdateViewModel updatedTrainer)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Provided data is invalid");
                return View(nameof(TrainerEdit), updatedTrainer);
            }
            bool isUpdated = _trainerService.UpdateTrainer(id, updatedTrainer);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Trainer

        public ActionResult DeleteTrainer(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Trainer can't be less than 1";
                return RedirectToAction(nameof(Index));
            }
            var trainer = _trainerService.GetTrainerById(id);
            if (trainer is not null)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Trainer";
            }
            ViewBag.TrainerId = id;
            ViewBag.TrainerName = trainer?.Name;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteTrainerConfirmed([FromForm] int id)
        {
            bool isDeleted = _trainerService.DeleteTrainer(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete Trainer";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
