using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class Project
    {
        [Key]
        public int ProjectID { get; set; }
        [Required]
        [Display(Name ="Project Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name ="Project Type")]
        public int ProjectTypeID { get; set; }
        [ForeignKey("ProjectTypeID")]
        public ProjectType ProjectType { get; set; }
        [Required]
        [Display(Name ="Client Name")]
        public string Client { get; set; }
        [Required]
        [Display(Name ="Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name ="End Date")]
        public DateTime EndDate { get; set; }
        [NotMapped]
        public string TypeName { get; set; }
        public List<ProjectType> List_ProjectType { get; set; }
        public List<ProjectTask> List_ProjectTask { get; set; }
        public List<Resource> List_ResourceNotAssign { get; set; }
        public List<Resource> List_ResourceAssign { get; set; }
        public List<TaskAssignResource> List_TaskAssignResource { get; set; }
        [NotMapped]
        public List<ActualTaskWork> List_actualTaskWorks { get; set; } 

    }
    public class ResourceAssign
    {
        [Key]
        public int ResourceAssignID { get; set; }
        public int ResourceID { get; set; }
        public int ProjectID { get; set; }

    }
    public class TaskAssignResource
    {
        public int TaskAssignResourceID { get; set; }
        [Required]
        public int ResourceAssignID { get; set; }
        [Required]
        public int ProjectTaskID { get; set; }
        [Required]
        public DateTime AssignDate { get; set; }
        [Required]
        public int AssignTime { get; set; }
        public int ProjectID { get; set; }
        [NotMapped]
        public int ActualTime { get; set; }
        [NotMapped]
        public string ResourceName { get; set; }
        [NotMapped]
        public string TaskName { get; set; }
        [NotMapped]
        public int status { get; set; }
    }
    public class ActualTaskWork
    {
        public int ActualTaskWorkID { get; set; }
        public int TaskAssignResourceID { get; set; }
        public int TimeTaken { get; set; }
        public int status { get; set; } //0- pending 1- working 2-completed
        public int ProjectTaskID { get; set; }
        [NotMapped]
        public int ProjectID { get; set; }
        [NotMapped]
        public string ResourceName { get; set; }
        [NotMapped]
        public string TaskName { get; set; }
       
       
    }
    public class ProjectType
    {
        public int ProjectTypeID { get; set; }
        [Required]
        [Display(Name ="Proejct Type Name")]
        public string TypeName { get; set; }
    }
    public class Resource
    {
        public int ResourceID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Designation { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public int ResourceAssignID { get; set; }
    }
    public class ProjectTask
    {
        [Key]
        public int ProjectTaskID { get; set; }
        public string TaskName { get; set; }
        public int ProjectID { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public int TaskTime { get; set; }
        [NotMapped]
        public int ActualTime { get; set; }
        [NotMapped]
        public int status { get; set; }
    }
    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
