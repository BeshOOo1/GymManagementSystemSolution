using AutoMapper;
using GymManagementBLL.Services.AttachmentService;
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
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if(Members == null || !Members.Any()) return[];

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
            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);

            #endregion
            return MemberViewModels;
        }

        public bool CreateMember(CreateMemberViewModel CreatedMember)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Member>();

                if (IsEmailExist(CreatedMember.Email))
                    return false;
                if (IsPhoneExist(CreatedMember.Phone))
                    return false;


                var PhotoName = _attachmentService.Upload("Members",CreatedMember.PhotoFile);

                if (string.IsNullOrEmpty(PhotoName))
                    return false;

                var MemberEntity = _mapper.Map<Member>(CreatedMember);
                MemberEntity.Photo = PhotoName;
                Repo.Add(MemberEntity);
                return _unitOfWork.SavaChanges() > 0;
            }
            catch
            {
                return false;
            }


        }

        public MemberViewModel? GetMemberDetials(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            var memberviewModel = _mapper.Map<MemberViewModel>(member);

            var ActiveMemberShip =_unitOfWork.GetRepository<MemberShip>()
                             .GetAll(X => X.MemberId == memberId && X.Status == "Active").FirstOrDefault();

            if(ActiveMemberShip is not null)
            {
                memberviewModel.MemberShipStartDate = ActiveMemberShip.CreatedAt.ToShortDateString();
                memberviewModel.MemberShipEndDate = ActiveMemberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActiveMemberShip.PlanId);
                memberviewModel.PlanName = plan?.Name;
            }
            return memberviewModel;
        }

        HealthRecordViewModel? IMemberService.GetHealthRecordDetails(int memberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId); // XXXXX

            if (MemberHealthRecord is null) return null;
            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if(member is null) return null;
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int id, MemberToUpdateViewModel updateMember)
        {
            try
            {

                var emailExit = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == updateMember.Email && X.Id != id).Any();

                var phoneExit = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == updateMember.Phone && X.Id != id).Any();
                if (emailExit || phoneExit) return false;
                
                var MemberRepo = _unitOfWork.GetRepository<Member>();
                var member = MemberRepo.GetById(id);
                if (member is null) return false;

                _mapper.Map(updateMember, member);
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
                var MemberShipRepo = _unitOfWork.GetRepository<MemberShip>();
                var member = MemberRepo.GetById(MemberId);
                if (member is null) return false;

                var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == MemberId).Select(X => X.SessionId);

                var ActiveMemberSession = _unitOfWork.GetRepository<Session>()
                    .GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

                if (ActiveMemberSession) return false;

                var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);

                if (MemberShips.Any())
                {
                    foreach (var membership in MemberShips)
                        MemberShipRepo.Delete(membership);
                }
                MemberRepo.Delete(member);
                var IsDeleted = _unitOfWork.SavaChanges() > 0;
                if (!IsDeleted)
                    _attachmentService.Delete(member.Photo, "Members");
                return IsDeleted;
            }
            catch
            {
                return false;
            }
        }
    }
}
