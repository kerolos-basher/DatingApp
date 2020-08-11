using DAtingApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.Data
{
    public class Seed
    {
        private readonly DataContext _context;

        public Seed(DataContext context)
        {
            _context = context;
           
        }
        public void SeedUsers()
        {
            var userdata = System.IO.File.ReadAllText("Data/seeduser.json");
            var users = JsonConvert.DeserializeObject<List<User>>(userdata);
            foreach (var user in users)
            {
                  byte[] PasswordHash, PasswordSalt;
                createPassswordHash("password", out PasswordHash, out PasswordSalt);
                user.PasswordSalt = PasswordSalt;
                user.PasswordHash = PasswordHash;
                user.Username = user.Username.ToLower();
                _context.Users.Add(user);
            }
            _context.SaveChanges();
        }
        private void createPassswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmak = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmak.Key;
                passwordHash = hmak.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}
