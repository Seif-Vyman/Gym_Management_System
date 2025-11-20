using GymManagementBLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAnalyticService _analyticService;

        public HomeController(IAnalyticService analyticService)
        {
            _analyticService = analyticService;
        }
        public IActionResult Index()
        {
            var Data = _analyticService.GetAnalyticData();
            //return View(); // return default [view with action name] view for action
            return View(Data); // return default view for action with passing model data
            //return View("Hamada"); // return spacific view for action
            //return View("hamada", Data) // return spacific view for action with passing model data;
        }
    }
}
