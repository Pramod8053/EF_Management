using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Entities
{
    public class tblMainMenu
    {   
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string MainMenu { get; set; }
    }
    public class tblSubMenu
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string SubMenu { get; set; }
        [MaxLength(50)]
        public string Controller { get; set; }
        [MaxLength(50)]
        public string Action { get; set; }
        public int MainMenuID { get; set; }
        [ForeignKey("MainMenuID")]
        public tblMainMenu tblMainMenu { get; set; }
    }

    public class tblSubMenuByRole
    {
        [Key]
        public int Id { get; set; }
        public int SubMenuId { get; set; }
        [ForeignKey("SubMenuId")]
        public tblSubMenu tblSubMenu { get; set; }
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public tblRoles tblRoles { get; set; }
        [NotMapped]
        public string MainMenuName { get; set; }
        [NotMapped]
        public string SubMenuName { get; set; }
        [NotMapped]
        public string Controller { get; set; }
        [NotMapped]
        public string Action { get; set; }
        [NotMapped]
        public bool Status { get; set; }
    }
}
