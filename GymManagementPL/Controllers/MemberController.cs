using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetMembers()
        {
            return View();
        }
        public ActionResult CreateMember()
        {
            return View();
        }
    }
}
