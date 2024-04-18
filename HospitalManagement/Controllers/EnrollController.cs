using HospitalManagement.Entities;
using HospitalManagement.Repositories.EnrollRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        #region  Patient 
        public IActionResult Patient()
        {
            if (TempData["msg"] != null)
            {
                ViewBag.msg = TempData["msg"];
            }
            ViewBag.Gender = _enrollrepo.GetGender();
            ViewBag.Specialists = _enrollrepo.GetSpecialists();
            return View(_enrollrepo.GetPatients());
        }
        [HttpPost]
        public IActionResult Patient(Patient _patient)
        {
            if (ModelState.IsValid)
            {
                ViewBag.msg = _enrollrepo.AddPatient(_patient);
            }
            else
            {
                ViewBag.msg = "Check the values.";
            }
            ViewBag.Specialists = _enrollrepo.GetSpecialists();
            ViewBag.Gender = _enrollrepo.GetGender();
            return View(_enrollrepo.GetPatients());
        }
        [HttpPost]
        public IActionResult GetPatientByID(int ID)
        {
            var doct = _enrollrepo.GetPatientById(ID);
            return Json(doct);
        }
        #endregion
        #region  Doctor
        public IActionResult Doctor()
        {
            ViewBag.Gender = _enrollrepo.GetGender();
            ViewBag.Specialist = _enrollrepo.GetSpecialists();
            return View(_enrollrepo.GetDoctors());
        }
        [Authorize(Roles = "Administrator,Manager")]
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
        #endregion
        #region Specialist
        public IActionResult Sepcialist()
        {
            return View(_enrollrepo.GetSpecialists());
        }
        [Authorize(Roles = "Administrator,Manager")]
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
        #endregion
        #region Appointment
        [HttpPost]
        public IActionResult AddAppointment(Appointment _app)
        {
            if (ModelState.IsValid)
            {
                _app.VisiteDate = DateTime.Now;
                TempData["msg"] = _enrollrepo.AddAppointment(_app);
            }
            else
            {
                TempData["msg"] = "Check the values.";
            }
           
            return RedirectToAction("Patient");
        }
        [HttpPost]
        public IActionResult GetDoctorByprofession(int Id)
        {
            var _doc = _enrollrepo.GetDoctors().FindAll(x=>x.SpecialistId == Id);
            return Json(_doc);
        }
        #endregion
        #region Doctor Visite
        public IActionResult DoctorAppointment()
        {
            var Doctorid = _enrollrepo.GetDoctors().FirstOrDefault(x=>x.Email == HttpContext.Session.GetString("UserName")).Id; 
            var _App = _enrollrepo.GetAppointments().FindAll(x=>x.DoctorId == Doctorid);
            return View(_App);
        }
        #endregion
    }
}
