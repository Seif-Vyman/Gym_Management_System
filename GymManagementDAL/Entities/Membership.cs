using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Membership : BaseEntity
    {
        // startedDate == CreatedAt of base entity

        public DateTime EndDate { get; set; }
        public String Status {
            get { return (DateTime.Now > EndDate) ? "Expired" : "Active"; }
        }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; }
    }
}
