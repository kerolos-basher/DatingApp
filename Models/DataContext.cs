using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace DAtingApp.Models
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext>option):base(option)
        {

        }
        public DbSet<Values> Values { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
