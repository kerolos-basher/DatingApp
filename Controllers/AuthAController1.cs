using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DAtingApp.DTO;
using DAtingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DAtingApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthAController1 : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
       
        public AuthAController1(DataContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        //--------------------------lognin----------------------------------------------------
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]UserTologin userTologin)
        {
            
            var userfromloginmethod = await Loginn(userTologin.Username.ToLower() , userTologin.Password);
            //if (userfromloginmethod == null)
            //{
            //    return BadRequest();
            //}
            //return Ok(userfromloginmethod);
            if (userfromloginmethod == null)
            {
                return Unauthorized();
            }
            var clims = new[]
         {
                new Claim(ClaimTypes.NameIdentifier,userfromloginmethod.Id.ToString()),
                new Claim(ClaimTypes.Name,userfromloginmethod.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var crids = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokendiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(clims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = crids
            };
            var tokernhandler = new JwtSecurityTokenHandler();
            var token = tokernhandler.CreateToken(tokendiscriptor);
            return Ok(new
            {
                token = tokernhandler.WriteToken(token)
            });
        }

        public async Task<User> Loginn(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return null;
                // return BadRequest("not found user");
            }
            if (!verifypassword(password, user.PasswordHash, user.PasswordSalt))
            {
                // return BadRequest("invalid  password");
                return null;
            }
            return user;
        }

        private bool verifypassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmak = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
               var newpasswordhash = hmak.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newpasswordhash.Length; i++)
                {
                    if (newpasswordhash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        //--------------------------register----------------------------------------------------
        [HttpPost("register")]
       // [Route("api/AuthAController1/Register")]
        public async Task<IActionResult> Register([FromBody]UserToRegister userToRegisterdto)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest("somthing rong");
            //}
            userToRegisterdto.Username = userToRegisterdto.Username.ToLower();
            if (await UserExists(userToRegisterdto.Username))
            {
                return BadRequest("User Allredy Exists");
            }
            var newuser = new User
            {
                Username = userToRegisterdto.Username
            };
            var createduser = await Registerr(newuser, userToRegisterdto.Password);
            return Ok();
        }
        public async Task<User> Registerr(User user, string password)
          
        {
              
          byte[] passwordHash, passwordSalt;
            createPassswordHash(password, out passwordHash, out passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        private void createPassswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmak = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmak.Key;
                passwordHash = hmak.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public async Task<bool> UserExists(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                return false;
            }
            return true;
        }
    }
}
