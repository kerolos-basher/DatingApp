using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAtingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DAtingApp.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ValueController1 : ControllerBase
    {
        // GET: ValueController1
       private  DataContext _context;
        public ValueController1(DataContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllValues()
        {
            var list = await _context.Values.ToListAsync();
            return Ok(list);
        }
        
       
        // [AllowAnonymous]
        [HttpGet("GetOne/{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var one = await _context.Values.FirstOrDefaultAsync(d=>d.ID==id);
            return Ok(one);
        }


    }
}
