using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.DTO
{
    public class photoForCreationDto
    {

        public string Url { get; set; }
        public IFormFile file { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public string PublicId { get; set; }
        public photoForCreationDto()
        {
            DateAdded = DateTime.Now;
        }
    }
}
