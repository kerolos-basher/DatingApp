using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.DTO
{
    public class UserToRegister
    {
        [Required]
        public string Username  { get; set; }
        [Required]
        [StringLength(8,MinimumLength =4,ErrorMessage ="you must Entrr password between 4 and 8 number")]
        public string Password { get; set; }
    }
    
}
