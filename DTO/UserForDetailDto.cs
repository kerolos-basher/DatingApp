using DAtingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.DTO
{
    public class UserForDetailDto
    {
        public int Id { get; set; }
        public String Username { get; set; }
       
        public String Gender { get; set; }
        public int Age { get; set; }
        public String KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActivated { get; set; }
        public String Introduction { get; set; }
        public String LookingFor { get; set; }
        public String Interrists { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public String PhotoUrl { get; set; }
        public ICollection<photoForDetaildDto> Photos { get; set; }
    }
}
