using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if(members == null || members.Any()) return[];

            #region Way 1

            //foreach (var member in members)
            //{ 
            //var memberViewModels = new MemberViewModel()
            //    { 
            //        Id = member.Id,
            //        Email = member.Email,
            //        Name = member.Name,
            //        Phone = member.Phone,
            //        Photo = member.Photo,
            //        Gender = member.Gender.ToString()
            //    };
            //    MemberViewModels.Add(memberViewModels);
            //}
            #endregion

            #region Way 2
            var MemberViewModels = members.Select(X => new MemberViewModel()
            {
                Name = X.Name,
                Email = X.Email,
                Id = X.Id,
                Phone = X.Phone,
                Photo = X.Photo,
                Gender = X.Gender.ToString()
            });

            #endregion
            return MemberViewModels;
        }

        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                // checked Email is Exist
                

                if (IsEmailExist(createMember.Email) || IsPhoneExist(createMember.Phone)) return false;

                // CreateMemberViewModel - Member => Mapping
                var member = new Member()
                {
                    Name = createMember.Name,
                    Email = createMember.Email,
                    Phone = createMember.Phone,
                    Gender = createMember.Gender,
                    DateOfBirth = createMember.DateOfBirth,
                    Address = new Address()
                    {
                        BildingNumber = createMember.BuildNumber,
                        City = createMember.City,
                        Street = createMember.Street
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.Weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note
                    }

                };
                _unitOfWork.GetRepository<Member>().Add(member);

                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public MemberViewModel? GetMemberDetials(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null) return null;

            var viewModel = new MemberViewModel()
            {
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BildingNumber} - {member.Address.Street} - {member.Address.City}",
            };

            var ActiveMemberShip =_unitOfWork.GetRepository<MemberShip>()
                             .GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();

            if(ActiveMemberShip is not null)
            {
                viewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }
            return viewModel;
        }

        HealthRecordViewModel? IMemberService.GetHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId); // XXXXX

            if (MemberHealthRecord == null) return null;

            // HealthRecord - HealthRecordViewModel
            return new HealthRecordViewModel()
            {
                Height = MemberHealthRecord.Height,
                BloodType = MemberHealthRecord.BloodType,
                Weight = MemberHealthRecord.Weight,
                Note = MemberHealthRecord.Note
            };
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if(member == null) return null;

            return new MemberToUpdateViewModel()
            {
                Photo = member.Photo,
                Phone = member.Phone,
                Name = member.Name,
                BuildNumber = member.Address.BildingNumber,
                City = member.Address.City,
                Street = member.Address.Street
            };
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
                if (IsEmailExist(memberToUpdate.Email) || IsPhoneExist(memberToUpdate.Phone)) return false;
                
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(id);
                if (member == null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BildingNumber = memberToUpdate.BuildNumber;
                member.Address.City = memberToUpdate.City;
                member.Address.Street = memberToUpdate.Street;

                member.UpdatedAt = DateTime.Now;

                MemberRepo.Update(member);
                return _unitOfWork.SavaChanges() > 0;

            }
            
            catch
            {
                return false;
            }
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
        #endregion

        public bool RemoveMember(int MemberId)
        {
            try
            {
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(MemberId);
                if (member == null) return false;

                var HasActiveMemberSessions = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == MemberId && X.Session.StartDate > DateTime.Now).Any();

                if (HasActiveMemberSessions) return false;

                var memberShipRepo = _unitOfWork.GetRepository<MemberShip>();
                var MemberShips = memberShipRepo.GetAll(X => X.MemberId == MemberId);
                if(MemberShips.Any())
                {
                    foreach (var membership in MemberShips)
                        memberShipRepo.Delete(membership);  
                }
                MemberRepo.Delete(member);
                return _unitOfWork.SavaChanges() > 0;

            }
            catch
            {
                return false;
            }
        }
    }
}
