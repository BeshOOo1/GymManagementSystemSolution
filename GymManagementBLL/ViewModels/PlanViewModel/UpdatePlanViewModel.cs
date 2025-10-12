using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.PlanViewModel
{
    public class UpdatePlanViewModel
    {

        public string PlanName { get; set; } = null!;


        [Required(ErrorMessage = "Description is Required")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Description Must be Between 5 And 50 Char")]
        public string Description { get; set; } = null!;


        [Required(ErrorMessage = "Duration Days is Required")]
        [Range(1,365,ErrorMessage ="Duration Days Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        [Range(0.1, 10000, ErrorMessage = "Price Between 0.1 and 10000")]
        public decimal Price { get; set; }

    }
}
