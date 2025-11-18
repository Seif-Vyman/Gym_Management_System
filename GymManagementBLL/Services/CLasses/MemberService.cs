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

        //private readonly IGenericRepository<Member> _memberRepository;
        //private readonly IGenericRepository<Membership> _membershipRepository;
        //private readonly IPlanRepository _planRepository;
        //private readonly IGenericRepository<HealthRecord> _healthRecordRepository;
        //private readonly IGenericRepository<MemberSession> _memberSessionRepository;

        // don't ask clr for creating object from service without register
        // CLR Will Inject address of object in constructor
        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members is null || !members.Any())
                return [];
            var memberViewModels = members.Select(m => new MemberViewModel
            {
                Id = m.Id,
                Photo = m.Photo,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Gender = m.Gender.ToString()
            }).ToList();
            return memberViewModels;
        }

        public MemberViewModel? GetMemberById(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null)
                return null;

            var viewModel = new MemberViewModel
            {
                Photo = member.Photo,
                Name = member.Name,
                Email = member.Email,
                Phone = member.Phone,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToString("yyyy-MM-dd"),
                Address = $"{member.Address.BuildingNumber}, {member.Address.Street}, {member.Address.City}",
            };

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

                var member = new Member()
                {
                    Name = createMemberViewModel.Name,
                    Email = createMemberViewModel.Email,
                    Phone = createMemberViewModel.Phone,
                    DateOfBirth = createMemberViewModel.DateOfBirth,
                    Gender = createMemberViewModel.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createMemberViewModel.buildingNumber,
                        Street = createMemberViewModel.Street,
                        City = createMemberViewModel.City
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMemberViewModel.HealthRecordViewModel.Height,
                        Weight = createMemberViewModel.HealthRecordViewModel.Weight,
                        BloodType = createMemberViewModel.HealthRecordViewModel.BloodType,
                        Note = createMemberViewModel.HealthRecordViewModel.Note
                    }

                };
                _unitOfWork.GetRepository<Member>().Add(member);
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
            return new HealthRecordViewModel
            {
                Height = memberHealthRecord.Height,
                Weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note

            };
        }

        public MemberToUpdateViewModel GetMemberToUpdate(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);
            if (member is null) return null;

            return new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Name = member.Name,
                Phone = member.Phone,
                Photo = member.Photo,
                buildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City
            };
        }

        public bool UpdateMemberDetails(int Id, MemberToUpdateViewModel updatedModel)
        {
            try
            {
                if (IsValidEmail(updatedModel.Email) || IsValidPhone(updatedModel.Phone)) return false;

                var member = _unitOfWork.GetRepository<Member>().GetById(Id);
                if (member is null) return false;

                member.Email = updatedModel.Email;
                member.Phone = updatedModel.Phone;
                member.Address.BuildingNumber = updatedModel.buildingNumber;
                member.Address.Street = updatedModel.Street;
                member.Address.City = updatedModel.City;
                member.UpdatedAt = DateTime.Now;
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
