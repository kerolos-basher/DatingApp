using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAtingApp.DTO;
using DAtingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController1 : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UsersController1(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

 
       
        public async Task<IActionResult> GetUsers()
        {
            var users = await _context.Users.Include(p=>p.Photos).ToListAsync();
            var userstoreturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
            return Ok(userstoreturn);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _context.Users.Include(p=>p.Photos).FirstOrDefaultAsync(u=>u.Id==id);
            var usertoReturn = _mapper.Map<UserForDetailDto>(user);
            return Ok(usertoReturn);
        }
        [HttpGet("GetUserbyname/{name}")]
        public async Task<IActionResult> GetUserbyname(string name)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Username == name);
            var usertoReturn = _mapper.Map<UserForDetailDto>(user);
            return Ok(usertoReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserForDetailDto userfordetail)
        {
            if (id == null)
            {
                return BadRequest();
            }
            if (id != userfordetail.Id)
            {
                return BadRequest();
            }
           var userfromdatabase= await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            userfromdatabase.Introduction = userfordetail.Introduction;
            userfromdatabase.Interrists = userfordetail.Interrists;
            userfromdatabase.LookingFor = userfordetail.LookingFor;
            userfromdatabase.City = userfordetail.City;
            userfromdatabase.Country = userfordetail.Country;

           await _context.SaveChangesAsync();
            return Ok();



        }
    }
}
