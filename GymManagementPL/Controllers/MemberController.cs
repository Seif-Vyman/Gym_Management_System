using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        #region GetAllMembers
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region Get Member Data

        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be less than 1";
                return RedirectToAction(nameof(Index));
            }

            var member = _memberService.GetMemberById(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }




        #endregion

        #region Get HealthRecord

        public ActionResult HealthRecord(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be less than 1";
                return RedirectToAction(nameof(Index));
            }
            var healthRecord = _memberService.GetMemberHealthRecord(id);
            if (healthRecord is null)
            {
                TempData["ErrorMessage"] = "Health record not found";
                return RedirectToAction(nameof(Index));
            }
            return View(healthRecord);
        }
        #endregion

        #region Create Member

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createdMember)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Provided data is invalid");
                return View(nameof(Create), createdMember);
            }
            bool isCreated = _memberService.CreateMember(createdMember);
            if(isCreated)
            {
                TempData["SuccessMessage"] = "Member created successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create member";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Edit Member

        public ActionResult MemberEdit(int id)
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member cannot be less than 1";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberToUpdate(id);
            if(member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id,MemberToUpdateViewModel memberEdit)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Provided data is invalid");
                return View(memberEdit);
            }
            bool isUpdated = _memberService.UpdateMemberDetails(id, memberEdit);
            if(isUpdated)
            {
                TempData["SuccessMessage"] = "Member updated successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of member can't be less than 1";
                return RedirectToAction(nameof(Index));
            }
            var member = _memberService.GetMemberById(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            ViewBag.MemberName = member.Name;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed([FromForm] int id)
        {
            var deleted = _memberService.RemoveMember(id);
            if(deleted)
            {
                TempData["SuccessMessage"] = "Member deleted successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete member";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}