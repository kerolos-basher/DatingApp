using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DAtingApp.Data;
using DAtingApp.DTO;

using DAtingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DAtingApp.Controllers
{
    [ApiController]
    [Route("api/users/{userId}/photos")]
    public class PhotoCon : ControllerBase
    {
        private readonly IMapper _mapper;
         private readonly IOptions<CloudinarySettings> _cloudinaryconfig;
        private Cloudinary _cloudinary;
        private readonly DataContext _context;

        public PhotoCon(IMapper mapper,IOptions<CloudinarySettings> cloudinaryconfig, DataContext context )
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryconfig = cloudinaryconfig;
            Account acc = new Account(
                _cloudinaryconfig.Value.CloudName,
                _cloudinaryconfig.Value.ApiKey,
                _cloudinaryconfig.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(acc);
        }
        [HttpGet("id", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photofromdb = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            var phot = _mapper.Map<PhotoToReturn>(photofromdb);
            return Ok(phot);
        }
        [HttpPost]
       
        public async Task<IActionResult> AddPhotoForUser(int userId,[FromForm] photoForCreationDto photoForCreationDto)
        {
            if (userId == null)
            {
                return BadRequest();
            }
            var userfromdatabase = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == userId);
            var file = photoForCreationDto.file;
            var aploadresult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadparams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                    };
                    aploadresult = _cloudinary.Upload(uploadparams);
                }
            }
            photoForCreationDto.Url = aploadresult.Uri.ToString();
            photoForCreationDto.PublicId = aploadresult.PublicId;
            var photoo = _mapper.Map<Photo>(photoForCreationDto);
            if (!userfromdatabase.Photos.Any(u => u.IsMain))
                photoo.IsMain = true;
            userfromdatabase.Photos.Add(photoo);
           
            if (await saveAll())
            {
                var photoforreturn = _mapper.Map<PhotoToReturn>(photoo);
                var photofromdb = await _context.Photos.FirstOrDefaultAsync(p => p.Id == photoo.Id);
                var phot = _mapper.Map<PhotoToReturn>(photofromdb);
                return Ok(photoforreturn);
            }
            return BadRequest("cant add phto");
        }
        public async Task<bool> saveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}

