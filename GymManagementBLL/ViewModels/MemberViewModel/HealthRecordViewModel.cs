using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is Required")]
        [Range(0.1, 300, ErrorMessage = "Height Must be Between 0.1 And 300")]
        public decimal Height { get; set; }


        [Required(ErrorMessage = "Height is Required")]
        [Range(0.1, 500, ErrorMessage = "weight Must be Between 0.1 And 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType is Required")]
        [StringLength(3, ErrorMessage = "BloodType Must be Between 3 char or 1 Less")]
        public string BloodType { get; set; } = null!;
        public string? Note { get; set; }
    }
}
