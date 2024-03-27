using EF_Registration.Models;
using EF_Registration.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Registration.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentRepo _repo;
        public HomeController(ILogger<HomeController> logger,IStudentRepo repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IActionResult Index()
        {
            List<Student> _stdList = _repo.GetStudent();
            return View(_stdList);
        }
        public IActionResult Create()
        {

            Student std = new Student();
            std.List_stateLists = _repo.GetStateList();
            std.List_districts = _repo.GetDistrict();
            return View(std);
        }
        [HttpPost]
        public IActionResult Create(Student std)
        {
            try
            {
                if(std.StudentImg != null && std.StudentImg.Length > 0)
                {
                   
                    var GetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PassportImg",std.StudentImg.FileName);
                    string extension = Path.GetExtension(GetPath);
                    if(extension ==".png" || extension == ".PNG")
                    {
                        using(var fileimg = new FileStream(GetPath,FileMode.Create))
                        {
                            std.StudentImg.CopyToAsync(fileimg);
                        }
                        std.PassportImg = std.StudentImg.FileName;
                        _repo.SaveStudent(std);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Only png file";
                    }
                  
                }
                else
                {
                    ViewBag.error = "File upload";
                }
               
            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;

            }
            std.List_stateLists = _repo.GetStateList();
            std.List_districts = _repo.GetDistrict().FindAll(x=>x.StateListID== std.StateListID);
            return View(std);
        }
        public IActionResult Delete(int id)
        {
            _repo.DeleteStudent(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult GetDistrict(int ID)
        {
            return Json(_repo.GetDistrict().FindAll(x=>x.StateListID==ID));
        }
       [HttpPost]
       public IActionResult getdata(int id)
        {
            return Json(34);
        }
        public IActionResult Edit(int id)
        {
            Student std = _repo.GetByIDStudent(id);
            std.List_stateLists = _repo.GetStateList();
            std.List_districts = _repo.GetDistrict().FindAll(x => x.StateListID == std.StateListID);
            return View(std);
        }
        [HttpPost]
        public IActionResult Edit(Student std)
        {
            try
            {
                if (std.StudentImg != null && std.StudentImg.Length > 0)
                {

                    var GetPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/PassportImg", std.StudentImg.FileName);
                    string extension = Path.GetExtension(GetPath);
                    if (extension == ".png" || extension == ".PNG")
                    {
                        using (var fileimg = new FileStream(GetPath, FileMode.Create))
                        {
                            std.StudentImg.CopyToAsync(fileimg);
                        }
                        std.PassportImg = std.StudentImg.FileName;
                        _repo.ModifyStudent(std);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.error = "Only png file";
                    }

                }
                else
                {
                    ViewBag.error = "File upload";
                }

            }
            catch (Exception ex)
            {
                ViewBag.error = ex.Message;

            }
            std.List_stateLists = _repo.GetStateList();
            std.List_districts = _repo.GetDistrict().FindAll(x => x.StateListID == std.StateListID);
            return View(std);
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
