using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member : GymUser
    {
        // joined date == createdat of baseentity
        public string? Photo { get; set; }

        #region Relationships

        public HealthRecord HealthRecord { get; set; } = null!;

        public ICollection<Membership> Memberships { get; set; } = null!;
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion
    }
}
