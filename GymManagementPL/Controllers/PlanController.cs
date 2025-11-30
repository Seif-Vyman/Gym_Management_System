using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public ActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }

        #region Plan Details
        public ActionResult Details(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanById(id);
            if(plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        #endregion

        #region Edit plan

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid plan Id";
                return RedirectToAction(nameof(Index));
            }
            var planToUpdate = _planService.GetPlanToUpdate(id);
            if (planToUpdate is null)
            {
                TempData["ErrorMessage"] = "Plan not found or cannot be updated";
                return RedirectToAction(nameof(Index));
            }
            return View(planToUpdate);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Provided data is invalid");
                return View(updatedPlan);
            }
            bool isUpdated = _planService.UpdatedPlan(id, updatedPlan);
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "Plan updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update plan";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region De/activate Plan

        [HttpPost]
        public ActionResult Activate(int id)
        {
            var result = _planService.TogglePlanStatus(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Plan status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = "Faile to change Plan Status";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
