using AutoMapper;
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
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainerAndCategories();
            if (Sessions is null || !Sessions.Any()) return [];

            #region Manual Mapping
            //return Sessions.Select(X => new SessionViewModel()
            //{
            //    Id = X.Id,
            //    Capacity = X.Capacity,
            //    Description = X.Description,
            //    StartDate = X.StartDate,
            //    EndDate = X.EndDate,
            //    TrainerName = X.SessionTrainer.Name,
            //    CategoryName = X.category.CategoryName,
            //    AvailableSlots = X.Capacity - _unitOfWork.SessionRepository.GetcountOfBookedSlots(X.Id)
            //});
            #endregion

            #region Automatic Mapping

            var MappedSessions = _mapper.Map<IEnumerable<Session>, IEnumerable<SessionViewModel>>(Sessions);
            return MappedSessions;
            #endregion

        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerAndCategories(id);
            if(Session is null) return null;
           
            var MappedSession = _mapper.Map<Session, SessionViewModel>(Session);
            return MappedSession;

        }
        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (!IsTrainerExists(createSession.TrainerId)) return false;
                if (!IsCategoryExists(createSession.CategoryId)) return false;
                if (!IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;

                var MappedSession = _mapper.Map<CreateSessionViewModel, Session>(createSession);

                _unitOfWork.SessionRepository.Add(MappedSession);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForUpdateing(Session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdateing(Session!)) return false;
                if (!IsTrainerExists(updateSession.TrainerId)) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;


                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);

                if (!IsSessionAvailableForRemoving(Session!)) return false;

                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        #region Helpers
        private bool IsTrainerExists(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;
        }
        private bool IsValidDateRange(DateTime StartDate ,DateTime EndDate)
        {
            return StartDate < EndDate && StartDate > DateTime.Now;
        }
        
        private bool IsSessionAvailableForUpdateing(Session session)
        {
            if (session == null) return false;

            if (session.EndDate < DateTime.Now) return false;

            if(session.StartDate <= DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetcountOfBookedSlots(session.Id) > 0;
            
            if(HasActiveBooking) return true;
            return false;
        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if (session == null) return false;

            if (session.StartDate > DateTime.Now) return false;

            if(session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now) return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetcountOfBookedSlots(session.Id) > 0;
            
            if(HasActiveBooking) return true;
            return false;
        }



        #endregion


    }
}
