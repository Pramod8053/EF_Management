using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class DBContextManager:DbContext
    {
        public DbSet<ProjectType> projectTypes { get; set; }
        public DbSet<Project> project { get; set; }
        public DbSet<Resource> resources { get; set; }
        public DbSet<ProjectTask> projectTasks { get; set; }
        public DbSet<ResourceAssign> resourceAssigns { get; set; }
        public DbSet<TaskAssignResource> taskAssignResources { get; set; }
        public DbSet<ActualTaskWork> actualTaskWorks { get; set; }
    }
}
