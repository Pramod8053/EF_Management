using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Registration.Models
{
    public class DBContextStudente : DbContext
    {
        public DbSet<Student> students { get; set; }
        public DbSet<StateList> stateLists { get; set; }
        public DbSet<District> districts { get; set; }
    }
}
