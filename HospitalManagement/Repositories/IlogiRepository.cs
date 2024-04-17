using HospitalManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repositories
{
   public interface IlogiRepository
    {
        List<tblRoles> GetRoles();
        List<tblLogin> GetLogin();

        List<tblMainMenu> GetMainMenu();
        List<tblSubMenuByRole> GetSubMenuByRole(int? RoleID);
        List<tblSubMenu> GetSubMenu();

        string AssignSubMenuByRole(tblSubMenuByRole _subMenu);
       
    }
}
