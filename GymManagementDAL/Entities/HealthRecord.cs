using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public string? BLoodType { get; set; }
        public string? Note { get; set; }

        #region Relationship

        #region MyRegion

        #endregion
        #endregion
    }
}
