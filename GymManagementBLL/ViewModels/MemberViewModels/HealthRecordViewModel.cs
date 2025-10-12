using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Range(0.1, 300, ErrorMessage = "Height must be between 0.1 and 300 cm.")]
        [Required(ErrorMessage = "Height is required.")]
        public decimal Height { get; set; }
        [Range(0.1, 500, ErrorMessage = "Height must be between 0.1 and 500 kg.")]
        [Required(ErrorMessage = "Weight is required.")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage = "Blood type is required.")]
        [Range(1, 3, ErrorMessage = "Blood type must be between 1 and 3 characters.")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; }
    }
}
