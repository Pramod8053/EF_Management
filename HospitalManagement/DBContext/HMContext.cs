using HospitalManagement.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.DBContext
{
    public class HMContt :DbContext
    {
        public DbSet<tblRoles> tblRoles { get; set; }
        public DbSet<tblLogin> tblLogins { get; set; }

        public DbSet<tblMainMenu> tblMainMenus { get; set; }
        public DbSet<tblSubMenu> tblSubMenus { get; set;}
        public DbSet<tblSubMenuByRole> tblSubMenuByRoles { get; set; }
        
        public DbSet<Gender> genders { get; set; }
        public DbSet<Specialist> specialists { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Appointment> appointments { get; set; }
    }
}
