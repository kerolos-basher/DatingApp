using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.Extentions
{
    public static class Extentions
    {
        public static void AddApplicationErrors(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Errror",message);
            response.Headers.Add("A-C-E-H","Application-Errror");
            response.Headers.Add("A-C-E-o", "*");
        }

        public static int calculateage(this DateTime thedatatime)
        {
            var age = DateTime.Today.Year - thedatatime.Year;
            if (thedatatime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }
    }
}
