using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.CLasses
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _maaper;

        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<Membership> _membershipRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;

        // don't ask clr for creating object from service without register
        // CLR Will Inject address of object in constructor
        public MemberService(IUnitOfWork unitOfWork, IMapper maaper)
        {
            _unitOfWork = unitOfWork;
            _maaper = maaper;
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any())
                return [];
            
            var MemberViewModels = _maaper.Map<IEnumerable<Member> , IEnumerable<MemberViewModel>>(members);
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberById(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;

            
            var viewModel = _maaper.Map<MemberViewModel>(member);
            // active membership
            var activeMembership = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == member.Id && m.Status == "Active").FirstOrDefault();
            if (activeMembership is not null)
            {
                var plan = _unitOfWork.GetRepository<Plan>().GetById(activeMembership.PlanId);
                viewModel.PlanName = plan?.Name;
                viewModel.MembershipStartDate = activeMembership.CreatedAt.ToString("yyyy-MM-dd");
                viewModel.MembershipEndDate = activeMembership.EndDate.ToString("yyyy-MM-dd");
            }
            return viewModel;
        }

        public bool CreateMember(CreateMemberViewModel createMemberViewModel)
        {
            try
            {
                if (IsValidEmail(createMemberViewModel.Email) || IsValidPhone(createMemberViewModel.Phone)) return false;

                var MemberEntity = _maaper.Map<Member>(createMemberViewModel); 
                _unitOfWork.GetRepository<Member>().Add(MemberEntity);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public HealthRecordViewModel? GetMemberHealthRecord(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetAll(hr => hr.Id == memberId).FirstOrDefault();
            if (memberHealthRecord is null)
                return null;
            
            return _maaper.Map<HealthRecordViewModel>(memberHealthRecord);
        }

        public MemberToUpdateViewModel GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            return _maaper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel updatedModel)
        {
            try
            {
                if (IsValidEmail(updatedModel.Email) || IsValidPhone(updatedModel.Phone)) return false;

                var member = _unitOfWork.GetRepository<Member>().GetById(Id);
                if (member is null) return false;

                _maaper.Map(updatedModel, member);
                _unitOfWork.GetRepository<Member>().Update(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveMember(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member is null) return false;
            var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>()
                .GetAll(m => m.MemberId == MemberId && m.Session.StartDate > DateTime.Now).Any();
            if (HasActiveMemberSession) return false;

            var memberships = _unitOfWork.GetRepository<Membership>().GetAll(m => m.MemberId == MemberId);
            try
            {
                if (memberships.Any())
                {
                    foreach (var membership in memberships)
                    {
                        _unitOfWork.GetRepository<Membership>().Delete(membership);
                    }
                }
                _unitOfWork.GetRepository<Member>().Delete(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }

        }

        #region Helper Methods

        private bool IsValidEmail(string email) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Email == email).Any();
        private bool IsValidPhone(string Phone) => _unitOfWork.GetRepository<Member>().GetAll(m => m.Phone == Phone).Any();

        

        #endregion
    }
}
