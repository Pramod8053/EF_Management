using HospitalManagement.Entities;
using HospitalManagement.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    [AllowAnonymous]
    public class loginController : Controller
    {
        private readonly IlogiRepository _loginRepo;
        private readonly ILogger<loginController> _logger;
        public loginController(IlogiRepository loginrepo, ILogger<loginController> logger)
        {
            _loginRepo = loginrepo; _logger = logger;
        }
        
        public IActionResult Index()
        {
            _logger.LogInformation("Login pages access...");
            _loginRepo.GetRoles();
            _loginRepo.GetMainMenu(); 
            return View();
        }
        [HttpPost]
        public IActionResult login(tblLogin _login)
        {
            if (ModelState.IsValid)
            {
                var _loginExist = _loginRepo.GetLogin().FindAll(x => x.UserName == _login.UserName && x.PassWord == _login.PassWord);
                if (_loginExist.Count > 0)
                {
                    // Set session value
                    HttpContext.Session.SetString("UserName", _loginExist.FirstOrDefault().UserName);
                    HttpContext.Session.SetString("RoleId", _loginExist.FirstOrDefault().RoleId.ToString());
                    HttpContext.Session.SetString("RoleName", _loginExist.FirstOrDefault().RoleName);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, _loginExist.FirstOrDefault().UserName),
                        new Claim(ClaimTypes.Role, _loginExist.FirstOrDefault().RoleName)
                        // Add more claims as needed
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    List<tblSubMenuByRole> _subMenu = _loginRepo.GetSubMenuByRole(_loginExist.FirstOrDefault().RoleId);
                    // Serialize the _subMenu list to JSON
                    string subMenuJson = JsonConvert.SerializeObject(_subMenu);
                    HttpContext.Session.SetString("Submenu", subMenuJson);
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index");
            
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}
