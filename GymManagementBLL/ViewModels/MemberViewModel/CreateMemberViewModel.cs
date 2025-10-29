﻿using GymManagementDAL.Entities.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class CreateMemberViewModel
    {
        [Required(ErrorMessage ="Photo is Required")]
        [Display(Name = "Profile Photo")]
        public IFormFile PhotoFile { get; set; } = null!;

        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 And 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name Can Contain only Letters And Spaces")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Invalid Email Format")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 And 100 Char")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [Phone(ErrorMessage = "Invalid Email Format")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010|011|012|015)\d{8}$",ErrorMessage ="Phome Number Must be Valid Egyption Phone Number")]    
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "DateOfBirth is Required")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }


        [Required(ErrorMessage = "Gender is Required")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Building Number is Required")]
        [Range(1,1000, ErrorMessage = "Building Number Must be Between 1 And 1000")]
        public int BuildNumber { get; set; }

        [Required(ErrorMessage = "Street is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 And 30 Char")]
        public string Street { get; set; } = null!;

        [Required(ErrorMessage = "City is Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 And 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City Can Contain only Letters And Spaces")]
        public string City { get; set; } = null!;
        public HealthRecordViewModel HealthRecordViewModel { get; set; } = null!;
    }
}
