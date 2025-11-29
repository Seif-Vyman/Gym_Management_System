using GymManagementBLL.Services.Interfaces;
using GymManagementSystemBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public ActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        #region Session Details

        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            return View(session);
        }

        #endregion

        #region Create Session

        public ActionResult Create()
        {
            LoadDropDownsForCategories();
            LoadDropDownsForTrainers();
            return View();
        }

        [HttpPost]
        public ActionResult Create(CreateSessionViewModel createdSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(createdSession);
            }
            var result = _sessionService.CreateSession(createdSession);
            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create session";
                LoadDropDownsForCategories();
                LoadDropDownsForTrainers();
                return View(createdSession);
            }
        }
        #endregion

        #region Edit Session

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session Id";
                return RedirectToAction(nameof(Index));
            }
            var sessionToUpdate = _sessionService.GetSessionToUpdate(id);
            if (sessionToUpdate is null)
            {
                TempData["ErrorMessage"] = "Session not found or cannot be updated";
                return RedirectToAction(nameof(Index));
            }
            LoadDropDownsForTrainers();
            return View(sessionToUpdate);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel updatedSession)
        {
            if (!ModelState.IsValid)
            {
                LoadDropDownsForTrainers();
                return View(updatedSession);
            }
            var result = _sessionService.UpdateSession(updatedSession, id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session";
                LoadDropDownsForTrainers();
                return View(updatedSession);
            }
        }


        #endregion

        #region Delete Session

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete session";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion



        #region Helper Methods 

        private void LoadDropDownsForCategories()
        {
            var categories = _sessionService.GetCategoryForDropDown();
            ViewBag.categories = new SelectList(categories, "Id", "Name");
        }
        private void LoadDropDownsForTrainers()
        {
            var trainers = _sessionService.GetTrainerForDropDown();
            ViewBag.trainers = new SelectList(trainers, "Id", "Name");
        }
        #endregion
    }
}
