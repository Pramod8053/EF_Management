using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Registration.Models
{
    public class StateList
    {
        public int StateListID { get; set; }
        public string StateName { get; set; }
    }
    public class District
    {
        public int DistrictID { get; set; }
        public string DistrictName {get;set;}
        public int StateListID { get; set; }
        [ForeignKey("StateListID")]
        public StateList StateList { get; set; }
    }
    public class Student
    {
        [Display(Name ="Slno.")]
        public int StudentID { get; set; }
        [MaxLength(50)]
        [Required]
        public string Name { get; set; }
        [Required]
        public int Age { get; set; }

        [Required]
        [Display(Name ="Mobile No.")]
        [MaxLength(10)]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a 10-digit mobile number")]
        public string MobileNo { get; set; }
        [Required]
        [Display(Name ="Email ID")]
        [EmailAddress]
        public string EmailID { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name ="State Name")]
        public int StateListID { get; set; }
        public string StateName { get; set; }
        [Required]
        [Display(Name ="District Name")]
        public int DistrictID { get; set; }
        public string DistrictName { get; set; }
       
        [Display(Name ="Upload Passport Image")]
        [Required]
        public IFormFile StudentImg { get; set; }
        public string PassportImg { get; set; }
        public List<StateList> List_stateLists { get; set; }
        public List<District> List_districts { get; set; }
    }
}
