using GymManagementBLL.ViewModels.MemberViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();
        bool CreateMember(CreateMemberViewModel createMemberViewModel);
        MemberViewModel? GetMemberById(int memberId);
        HealthRecordViewModel? GetMemberHealthRecord(int memberId);

        MemberToUpdateViewModel GetMemberToUpdate(int memberId);

        bool UpdateMemberDetails(int Id,MemberToUpdateViewModel updatedModel);

        bool RemoveMember(int MemberId);
    }
}
