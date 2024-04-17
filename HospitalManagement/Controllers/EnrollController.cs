using HospitalManagement.Entities;
using HospitalManagement.Repositories.EnrollRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Controllers
{
    public class EnrollController : Controller
    {
        private readonly IEnrollRepo _enrollrepo;
        public EnrollController(IEnrollRepo repo)
        {
            _enrollrepo = repo;
        }
        public IActionResult Patient()
        {
            return View(_enrollrepo.GetPatients());
        }
        public IActionResult Doctor()
        {
            ViewBag.Gender = _enrollrepo.GetGender();
            ViewBag.Specialist = _enrollrepo.GetSpecialists();
            return View(_enrollrepo.GetDoctors());
        }
        [HttpPost]
        public IActionResult Doctor(Doctor _doctor)
        {
            if (ModelState.IsValid)
            {
                ViewBag.msg = _enrollrepo.AddDoctor(_doctor);
            }
            else
            {
                ViewBag.msg = "Check the values.";
            }
            ViewBag.Gender = _enrollrepo.GetGender();
            ViewBag.Specialist = _enrollrepo.GetSpecialists();
            return View(_enrollrepo.GetDoctors());
        }
        [HttpPost]
        public IActionResult GetDoctorByID(int ID)
        {
            var doct = _enrollrepo.GetDoctorByID(ID);
            return Json(doct);
        }
        public IActionResult Sepcialist()
        {
            return View(_enrollrepo.GetSpecialists());
        }
       [HttpPost]
        public IActionResult Sepcialist(Specialist _specialist)
        {
            if (ModelState.IsValid)
            {
                ViewBag.msg = _enrollrepo.AddSpecialist(_specialist);
            }
            else
            {
                ViewBag.msg = ModelState.ErrorCount.ToString();
            }
            
            return View(_enrollrepo.GetSpecialists());
        }
    }
}
