using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
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
        #region Get All Members
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }
        #endregion

        #region GetMember Data
        public ActionResult MemberDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetials(id);

            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Fount";
                return RedirectToAction(nameof(Index));
            }

            return View(Member);
        }

        public ActionResult HealthRecordDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of HealthRecord can not be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var HealthRecord = _memberService.GetHealthRecordDetails(id);

            if (HealthRecord is null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Fount";
                return RedirectToAction(nameof(Index));
            }

            return View(HealthRecord);
        }
        #endregion

        #region Add Member
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel createMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInvalid", "Check Data And Missing Fields");
                return View(nameof(Create), createMember);
            }
            bool Result = _memberService.CreateMember(createMember);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfuly";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to Create , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Ubdate Member

        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberToUpdate(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Fount";
                return RedirectToAction(nameof(Index));
            }
            return View(Member);
        }
        [HttpPost]
        public ActionResult MemberEdit([FromRoute] int id ,MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var Result = _memberService.UpdateMemberDetails(id, viewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfuly";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to Update , Check Phone And Email";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
        
        #region Delete Member

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Id of Member can not be 0 or Nigative Number";
                return RedirectToAction(nameof(Index));
            }

            var Member = _memberService.GetMemberDetials(id);
            if (Member is null)
            {
                TempData["ErrorMessage"] = "Member Not Fount";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id; 
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int id)
        {
            var Result = _memberService.RemoveMember(id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Member Delete Successfuly";

            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed to Delete ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

    }
}
