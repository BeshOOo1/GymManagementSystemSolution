using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModel
{
    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage ="Name is Required")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contain only Letters And Spaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage = "Invalid Email Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Phome Number Must be Valid Egyption Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number Must be Greater than 0")]
        public int BuildNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(150, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 And 150 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street Can Contain only Letters And Spaces")]

        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City Must be Between 2 And 100 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain only Letters And Spaces")]
        public string City { get; set; } = null!;

        public Specialties Specialties { get; set; }
    }
}
