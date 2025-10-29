using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.AccountViewModel
{
    public class AccountViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Email { get; set; } = null!;
       [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.Password)]
        public string Passward { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
