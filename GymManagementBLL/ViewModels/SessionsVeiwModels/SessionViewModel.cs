using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.SessionsVeiwModels
{
    public class SessionViewModel
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string TrainerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Capcity { get; set; }
        public int AvailableSlots { get; set; }
        #region Computed Properties

        public string DateDisplay => $"{StartDate:MMM dd, yyyy}";
        public string TimeRangeDisplay => $"{StartDate:hh:mm tt} - {EndDate:hh:mm tt}";

        public string Status
        {             get
            {
                var currentDate = DateTime.Now;
                if (currentDate < StartDate)
                    return "Upcoming";
                else if (currentDate >= StartDate && currentDate <= EndDate)
                    return "Ongoing";
                else
                    return "Completed";
            }
        }

        #endregion
    }
}
