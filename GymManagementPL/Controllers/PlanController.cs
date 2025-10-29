using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        public ActionResult Details(int id, string ViewName = "Details")
        {
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanById(id);
            if(Plan == null)
            {
                TempData["ErrorMessage"] = " Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(ViewName,Plan);
        }
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var Plan = _planService.GetPlanToUpdate(id);
            if (Plan == null)
            {
                TempData["ErrorMessage"] = " Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdatePlanViewModel updatePlan)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation");
                return View(updatePlan);
            }
            var Result = _planService.UpdatePlan(id, updatePlan);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Is Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed to Update";

            }
            return RedirectToAction(nameof(Index));
        }
        public ActionResult Activate(int id)
        {
            var Result = _planService.ToggleStatus(id);
            if(Result)
            {
                TempData["SuccessMessage"] = "Plan Status Changed";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Change Plan Status";
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
