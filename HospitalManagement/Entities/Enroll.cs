using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Entities
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string PatientName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public int Gender { get; set; }
        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
       
        [NotMapped]
        public string GenderName { get; set; }
        [NotMapped]
        public List<Appointment> List_Appointment { get; set; }


    }
    public class Doctor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string DoctorName { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public int GenderId { get; set; }
        [Required]
        [MaxLength(10)]
        public string Mobile { get; set; }
        [Required]
        [MaxLength(100)]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        [Required]
        [MaxLength(250)]
        public string Address { get; set; }
        [Required]
        public int SpecialistId { get; set; }
        [ForeignKey("SpecialistId")]
        public Specialist Specialist { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }
        [NotMapped]
        public string GenderName { get; set; }
        [NotMapped]
        public string SpecialistName { get; set; }
    }
    public class Specialist
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Name { get; set; }
        public string Qualification { get; set; }
        [NotMapped]
        public int DoctorCount { get; set; }
        [NotMapped]
        public List<Doctor> List_doctor { get; set; }
    }
    public class Gender
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        [Required]
        [MaxLength(250)]
        public string ReasonForVisite { get; set; }
        public DateTime VisiteDate { get; set; }
        public bool status { get; set; } = false; 
        [NotMapped]
        public string DoctorName { get; set; }
        [NotMapped]
        public string PatientName { get; set; }
    }
}
