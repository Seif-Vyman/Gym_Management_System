using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModels
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "Name must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; } = null!;

        [Required (ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DataType(DataType.EmailAddress)] // ui hint
        [StringLength(100,MinimumLength = 5 ,ErrorMessage = "Email must be between 5 and 100 char.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phone must be balid Egyptian phone number.")]
        [DataType(DataType.PhoneNumber)] // ui hint
        public string Phone { get; set; }

        [Required(ErrorMessage = "Date Of Birth is required"]
        [DataType(DataType.Date)] // ui hint
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building number is required")]
        [Range(1, 9000, ErrorMessage = "Building number must be between 1 and 9000.")]
        public int buildingNumber { get; set; }

        [Required(ErrorMessage = "Street is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street must be between 2 and 30 characters.")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City must be between 2 and 30 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City can only contain letters and spaces.")]
        public string City { get; set; } = null!;

        [Required(ErrorMessage = "Health record is required.")]
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
