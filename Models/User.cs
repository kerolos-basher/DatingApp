using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public String Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public String Gender { get; set; }
        public DateTime DateOfBirth  { get; set; }
        public String KnownAs { get; set; }
        public DateTime Created  { get; set; }
        public DateTime LastActivated  { get; set; }
        public String  Introduction { get; set; }
        public String LookingFor  { get; set; }
        public String Interrists { get; set; }
        public String City { get; set; }
        public String Country { get; set; }
        public ICollection<Photo> Photos { get; set; }
    }
}
