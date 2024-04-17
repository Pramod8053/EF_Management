using HospitalManagement.DBContext;
using HospitalManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repositories
{
    public class logiRepository : IlogiRepository
    {
       
        private readonly HMContextt _dbcontext;
        public logiRepository()
        {
            _dbcontext =new HMContextt();
        }
        public List<tblLogin> GetLogin()
        {
          
          var  _tblLogins = (from login in _dbcontext.tblLogins.ToList() 
                         join role in _dbcontext.tblRoles.ToList() on login.RoleId equals role.Id
                         select new tblLogin {
                                    Id= login.Id ,
                                    UserName = login.UserName,
                                    PassWord =login.PassWord ,
                                    RoleId = login.RoleId ,
                                    RoleName = role.Roles 
                          }).ToList();
            return _tblLogins;
        }

        public List<tblRoles> GetRoles()
        {
            List<tblRoles> _tblRoles = new List<tblRoles>() {
            new tblRoles{Id=1,Roles="Administrator"},
            new tblRoles {Id=2,Roles="Manager"},
            new tblRoles{Id=3,Roles="User" }
            };
            foreach(var item in _tblRoles)
            {
                if(_dbcontext.tblRoles.ToList().FirstOrDefault(x=>x.Roles == item.Roles)==null)
                {
                    _dbcontext.tblRoles.Add(item);
                }
            }
            _dbcontext.SaveChanges();

            List<tblLogin> _tblLogins = new List<tblLogin>()
            {
                new tblLogin{Id=1,UserName="Admin",PassWord="Admin123",RoleId=1},
                new tblLogin{Id=1,UserName="Manager",PassWord="Manager123",RoleId=2},
                new tblLogin{Id=1,UserName="User",PassWord="User123",RoleId=3}
            };

            foreach (var item in _tblLogins)
            {
                if (_dbcontext.tblLogins.ToList().FindAll(x => x.UserName == item.UserName).Count == 0)
                {
                    _dbcontext.tblLogins.Add(item);
                }
            }
            _dbcontext.SaveChanges();

            return _dbcontext.tblRoles.ToList();
        }
        public List<tblMainMenu> GetMainMenu()
        {
            List<tblMainMenu> _mainmenu = new List<tblMainMenu>(){
               new tblMainMenu { Id = 1, MainMenu = "Home"},
               new tblMainMenu { Id = 2, MainMenu = "Sales" },
               new tblMainMenu { Id = 3, MainMenu = "Purchase" },
               new tblMainMenu { Id = 4, MainMenu = "Report" },
               new tblMainMenu { Id = 5, MainMenu = "Enroll" },
            };
            foreach(var item in _mainmenu)
            {
                if(_dbcontext.tblMainMenus.ToList().FindAll(x=>x.MainMenu== item.MainMenu).Count == 0)
                {
                    _dbcontext.tblMainMenus.Add(item);
                }
            }
            _dbcontext.SaveChanges();

            List<tblSubMenu> _submenu = new List<tblSubMenu>() {
                     new tblSubMenu{Id=1,SubMenu="Index",Controller="Home",Action="Index",MainMenuID=1 },
                     new tblSubMenu{Id=2,SubMenu="Roles",Controller="Home",Action="Roles",MainMenuID=1 },
                     new tblSubMenu{Id=3,SubMenu="New Invoice",Controller="Sales",Action="Invoice",MainMenuID=2},
                     new tblSubMenu{Id=4,SubMenu="Drugs",Controller="Purchase",Action="Drugs",MainMenuID=3},
                     new tblSubMenu{Id=5,SubMenu="Patient",Controller="Enroll",Action="Patient",MainMenuID=5},
                     new tblSubMenu{Id=6,SubMenu="Doctor",Controller="Enroll",Action="Doctor",MainMenuID=5},
                     new tblSubMenu{Id=7,SubMenu="Sepcialist",Controller="Enroll",Action="Sepcialist",MainMenuID=5},

            };
            foreach (var item in _submenu)
            {
                if (_dbcontext.tblSubMenus.ToList().FindAll(x => x.SubMenu == item.SubMenu).Count == 0)
                {
                    _dbcontext.tblSubMenus.Add(item);
                }
            }
            _dbcontext.SaveChanges();

            if (_dbcontext.tblSubMenuByRoles.ToList().FindAll(x => x.RoleID == 1 && x.SubMenuId == 2).Count == 0)
            {
                tblSubMenuByRole _role = new tblSubMenuByRole();
                _role.SubMenuId = 2;
                _role.RoleID = 1;
                _role.Status = true;
                AssignSubMenuByRole(_role);

            }

            return _dbcontext.tblMainMenus.ToList();
        }
        public List<tblSubMenu> GetSubMenu()
        {

            return _dbcontext.tblSubMenus.ToList();
        }

        public List<tblSubMenuByRole> GetSubMenuByRole(int? RoleID)
        {
           
            var _submenu = (from sbr in _dbcontext.tblSubMenuByRoles.ToList().FindAll(x=>x.RoleID == RoleID)
                            join sb in _dbcontext.tblSubMenus on sbr.SubMenuId equals sb.Id
                            join mm in _dbcontext.tblMainMenus on sb.MainMenuID equals mm.Id
                            select new tblSubMenuByRole
                            {
                                Id = sbr.Id,
                                SubMenuId = sbr.SubMenuId,
                                RoleID = sbr.RoleID,
                                MainMenuName = mm.MainMenu,
                                SubMenuName = sb.SubMenu,
                                Controller = sb.Controller,
                                Action = sb.Action
                            }).ToList();
            return _submenu;
        }

        public string AssignSubMenuByRole(tblSubMenuByRole _subMenu)
        {
            if(_dbcontext.tblSubMenuByRoles.ToList().FindAll(x=>x.RoleID == _subMenu.RoleID && x.SubMenuId == _subMenu.SubMenuId).Count > 0 && _subMenu.Status == true)
            {
                return "Already Exist";
            }
            else
            {
                if(_subMenu.Status == true)
                {
                    _dbcontext.tblSubMenuByRoles.Add(_subMenu);
                }
                else
                {
                    tblSubMenuByRole _smr = _dbcontext.tblSubMenuByRoles.FirstOrDefault(x => x.RoleID == _subMenu.RoleID && x.SubMenuId == _subMenu.SubMenuId);
                    if(_smr != null)
                    {
                        _dbcontext.tblSubMenuByRoles.Remove(_smr);
                    }
                    else
                    {
                        return "Not Avaiable";
                    }
                   
                }
              
                _dbcontext.SaveChanges();
            }
          
            return "Save";
        }
    }
}
