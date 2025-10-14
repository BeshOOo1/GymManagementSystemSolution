using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategories();
            if (Sessions is null || !Sessions.Any()) return [];

            return Sessions.Select(X => new SessionViewModel()
            {
                Id = X.Id,
                Capaicty = X.Capacity,
                Description = X.Description,
                StartDate = X.StartDate,
                EndDate = X.EndDate,
                TrainerName = X.SessionTrainer.Name,
                CategoryName = X.category.CategoryName,
                AvailableSlots = X.Capacity - _unitOfWork.SessionRepository.GetcountOfBookedSlots(X.Id)
            });
        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategories(id);
            if(Session is null) return null;
            return new SessionViewModel()
            { 
                Capaicty = Session.Capacity,
                Description = Session.Description,
                StartDate = Session.StartDate,
                EndDate = Session.EndDate,
                CategoryName = Session.category.CategoryName,
                TrainerName = Session.SessionTrainer.Name,
                AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetcountOfBookedSlots(Session.Id)
            };


        }
    }
}
