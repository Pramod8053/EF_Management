using HospitalManagement.Entities;
using HospitalManagement.Models;
using HospitalManagement.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
  
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IlogiRepository _loginRepo;
        public HomeController(ILogger<HomeController> logger, IlogiRepository loginRepo)
        {
            _logger = logger; _loginRepo = loginRepo;
        }
     
        public IActionResult Index()
        {
            _logger.LogInformation("Welcome");
            return View();
        }
       
        public IActionResult Roles()
        {
            return View(_loginRepo.GetRoles());
        }
        public IActionResult AssignThePages(int RollId)
        {
            tblRoles _roles = _loginRepo.GetRoles().FirstOrDefault(x=>x.Id == RollId);
            if (_roles!=null)
            {
                var _submenu = from sb in _loginRepo.GetSubMenu()
                               join mm in _loginRepo.GetMainMenu() on sb.MainMenuID equals mm.Id
                               join smr in _loginRepo.GetSubMenuByRole(RollId) on sb.Id equals smr.SubMenuId into _menuGroup
                               from smr in _menuGroup.DefaultIfEmpty()
                               select new tblSubMenuByRole
                               {
                                   SubMenuId = sb.Id,
                                   MainMenuName = mm.MainMenu,
                                   SubMenuName = sb.SubMenu,
                                   Controller = sb.Controller,
                                   Action = sb.Action,
                                   RoleID = RollId,
                                   Status = smr != null ? true : false
                               };


                _roles.List_SubMenuByRole = _submenu.ToList();
            }
            return View(_roles);
        }
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public IActionResult AssignThePages(tblRoles _Roll)
        {
            foreach(var item in _Roll.List_SubMenuByRole)
            {
              _logger.LogInformation(_loginRepo.AssignSubMenuByRole(item));
            }
            if (_Roll.List_SubMenuByRole.FindAll(x => x.Status == true).Count > 0)
            {
                TempData["msg"] = "Assign the pages successfully";
            }
            
            return RedirectToAction("AssignThePages",new { RollId = _Roll.Id });
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
