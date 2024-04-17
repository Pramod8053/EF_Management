using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Entities
{
    public class tblRoles
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string Roles { get; set; }
        [NotMapped]
        public List<tblSubMenuByRole> List_SubMenuByRole { get; set; }
    }
    public class tblLogin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter the User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter the Password")]
        [DataType(DataType.Password)]
        public string PassWord { get; set; }
        public int? RoleId { get; set; }
        [ForeignKey("RoleId")]
        public tblRoles tblRoles { get; set; }
        [NotMapped]
        public string RoleName { get; set; }
    }
}
