using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.PlanViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class PlanService : IPlanService
    {
        private readonly UnitOfWork _unitOfWork;

        public PlanService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
            var Plans = _unitOfWork.GetRepository<Plan>().GetAll();
            if (Plans == null || !Plans.Any()) return [];

            return Plans.Select(X => new PlanViewModel()
            {
                Description = X.Description,
                DurationDays = X.DurationDays,
                Id = X.Id,
                IsActive = X.IsActive,
                Name = X.Name,
                Price = X.Price
            });
        }

        public PlanViewModel? GetPlanDetails(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (Plan == null) return null;

            return new PlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                Id = Plan.Id,
                IsActive = Plan.IsActive,
                Name = Plan.Name,
                Price = Plan.Price
            };

        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
            var Plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);

            if (Plan is null || Plan.IsActive == false || HasActiveMemberShips(PlanId)) return null;
            return new UpdatePlanViewModel()
            {
                Description = Plan.Description,
                DurationDays = Plan.DurationDays,
                PlanName = Plan.Name,
                Price = Plan.Price
            };
        }

        public bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatePlan)
        {
            try
            {
                var PlanRepo = _unitOfWork.GetRepository<Plan>();
                var Plan = PlanRepo.GetById(PlanId);
                if (Plan is null || HasActiveMemberShips(PlanId)) return false;

                (Plan.Description, Plan.Price, Plan.DurationDays, Plan.UpdatedAt) =
                                 (UpdatePlan.Description, UpdatePlan.Price, UpdatePlan.DurationDays, DateTime.Now);
                PlanRepo.Update(Plan);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }
        }
        public bool ToggleStatus(int PlanId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var Plan = PlanRepo.GetById(PlanId);
            if (Plan is null || HasActiveMemberShips(PlanId)) return false;

            Plan.IsActive = Plan.IsActive == true ? false :true;
            try
            {
                PlanRepo.Update(Plan);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }

        }


        #region Helper Methods
        private bool HasActiveMemberShips(int PlanId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                .GetAll(X => X.PlanId == PlanId && X.Status == "Active").Any();
        }
        #endregion
    }
}
