using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone)) return false;

                var Trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Specialties = createTrainer.Specialties,
                    Gender = createTrainer.Gender,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Address = new Address()
                    {
                        BildingNumber = createTrainer.BuildNumber,
                        City = createTrainer.City,
                        Street = createTrainer.Street,
                    }
                };
                Repo.Add(Trainer);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainer()
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (Trainer is null || !Trainer.Any()) return [];

            return Trainer.Select(X => new TrainerViewModel()
            {
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Id = X.Id,
                Specialties = X.Specialties.ToString()
            });

        }

        public TrainerViewModel? GetTrainerDetails(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null ) return null;

            return new TrainerViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                Specialties = Trainer.Specialties.ToString()
            };

        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int trainerId)
        {
            var Trainer = _unitOfWork.GetRepository<Trainer>().GetById(trainerId);
            if (Trainer is null) return null;

            return new TrainerToUpdateViewModel()
            {
                Name = Trainer.Name,
                Email = Trainer.Email,
                Phone = Trainer.Phone,
                City = Trainer.Address.City,
                Street = Trainer.Address.Street,
                BuildNumber = Trainer.Address.BildingNumber,
                Specialties = Trainer.Specialties
            };

        }

        public bool RemoveTrainer(int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(trainerId);
            if (TrainerToRemove is null || HasActiveSessions(trainerId)) return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SavaChanges() > 0;
        }



        public bool UpdateTrainerDetails(UpdateTrainerViewModel updateTrainer, int trainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(trainerId);
            if (TrainerToUpdate is null || IsEmailExist(updateTrainer.Email) || IsPhoneExist(updateTrainer.Phone)) return false;

            TrainerToUpdate.Email = updateTrainer.Email;
            TrainerToUpdate.Phone = updateTrainer.Phone;
            TrainerToUpdate.Address.BildingNumber = updateTrainer.BuildNumber;
            TrainerToUpdate.Address.City = updateTrainer.City;
            TrainerToUpdate.Address.Street = updateTrainer.Street;
            TrainerToUpdate.Specialties = updateTrainer.Specialties;
            TrainerToUpdate.UpdatedAt = DateTime.Now;
            Repo.Update(TrainerToUpdate);

            return _unitOfWork.SavaChanges() > 0;
        }

        #region Helper Methods

        private bool IsEmailExist(string Email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email).Any();
        }
        private bool IsPhoneExist(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone).Any();
        }


        private bool HasActiveSessions(int trainerId)
        {
            return _unitOfWork.GetRepository<Session>()
                .GetAll(X => X.TrainerId == trainerId && X.Description == "Active").Any();        }
        #endregion
    }
}
