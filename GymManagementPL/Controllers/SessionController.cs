using AspNetCoreGeneratedDocument;
using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    [Authorize]
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionById(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = "Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(Session);
        }
        public ActionResult Create()
        {
            LoadTrainersDropDowns();
            LoadCategoriesDropDowns();
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(viewModel);
            }
            var Result = _sessionService.CreateSession(viewModel);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Created Successfuly";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed to Create";
                LoadTrainersDropDowns();
                LoadCategoriesDropDowns();
                return View(viewModel);

            }
            
        }
        public ActionResult Edit(int id)
        { 
            if(id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var Session = _sessionService.GetSessionToUpdate(id);
            if (Session == null)
            {
                TempData["ErrorMessage"] = " Session Not Found";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainersDropDowns();
            return View(Session);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute] int id, UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainersDropDowns();
                return View(updateSession);
            }
            var Result = _sessionService.UpdateSession(updateSession, id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Session Is Update Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Session Failed to Update";
            }
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.SessionId = session.Id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var result = _sessionService.RemoveSession(id);

            if (result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Session cannot be deleted";
            }
            return RedirectToAction(nameof(Index));
        }
        #region Helpers
        private void LoadTrainersDropDowns()
        {
            var Trainers = _sessionService.GetAllTrainersForDropDown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        private void LoadCategoriesDropDowns()
        {
            var Categories = _sessionService.GetAllCategoryForDropDown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
        }
        #endregion
    }
}
