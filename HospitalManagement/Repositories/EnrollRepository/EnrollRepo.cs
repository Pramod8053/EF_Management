using HospitalManagement.DBContext;
using HospitalManagement.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repositories.EnrollRepository
{
    public class EnrollRepo : IEnrollRepo
    {
        private readonly HMContt _dbcontext;
        private readonly ILogger<EnrollRepo> _logger;
        public EnrollRepo(ILogger<EnrollRepo> logger)
        {
            _dbcontext = new HMContt();
            _logger =logger;
            GetGender();
        }
        #region Doctor
        public string AddDoctor(Doctor _doctor)
        {
            try { 
                if(_doctor.Id != 0)
                {
                    Doctor doct = _dbcontext.doctors.ToList().FirstOrDefault(x => x.Id == _doctor.Id);
                    doct.DoctorName = _doctor.DoctorName;
                    doct.Age = _doctor.Age;
                    doct.GenderId = _doctor.GenderId;
                    doct.SpecialistId = _doctor.SpecialistId;
                    doct.Mobile = _doctor.Mobile;
                    doct.Email = _doctor.Email;
                    doct.Address = _doctor.Address;
                    _dbcontext.SaveChanges();
                    return "Updated successfully.";
                }
                else
               if(_dbcontext.doctors.ToList().FirstOrDefault(x=>x.DoctorName == _doctor.DoctorName || x.Email == _doctor.Email) == null)
                {
                    _dbcontext.doctors.Add(_doctor);
                    _dbcontext.SaveChanges();
                    return "Add successfully.";
                }
            else
            {
                return "Record Exists.";
            }
            }catch(Exception ex)
            {
                _logger.LogError("AddDoctor",ex.ToString());
                return ex.ToString();
            }


        }
        public List<Doctor> GetDoctors()
        {
            var _doctor = from doc in _dbcontext.doctors.ToList()
                          join gen in _dbcontext.genders.ToList() on doc.GenderId equals gen.Id
                          join spe in _dbcontext.specialists.ToList() on doc.SpecialistId equals spe.Id
                          select new Doctor
                          {
                              Id = doc.Id,
                              DoctorName = doc.DoctorName,
                              Age = doc.Age,
                              GenderId = doc.GenderId,
                              GenderName = gen.Name,
                              Mobile = doc.Mobile,
                              Email =doc.Email,
                              Address = doc.Address,
                              SpecialistId = doc.SpecialistId,
                              SpecialistName = spe.Name
                          };
            return _doctor.ToList();
        }
        public Doctor GetDoctorByID(int Id)
        {
            return GetDoctors().FirstOrDefault(x => x.Id == Id);
        }
        #endregion
        #region Patient
        public string AddPatient(Patient _patient)
        {
            try
            {
                if (_patient.Id != 0)
                {
                    Patient _Patient = _dbcontext.patients.ToList().FirstOrDefault(x => x.Id == _patient.Id);
                    _Patient.PatientName = _patient.PatientName;
                    _Patient.Age =          _patient.Age;
                    _Patient.Gender =     _patient.Gender;
                    _Patient.Mobile =       _patient.Mobile;
                    _Patient.Address =      _patient.Address;
                    _dbcontext.SaveChanges();
                    return "Updated successfully.";
                }
                else
                if (_dbcontext.patients.FirstOrDefault(x => x.PatientName == _patient.PatientName && x.Mobile == _patient.Mobile) == null)
                {
                    _dbcontext.patients.Add(_patient);
                    _dbcontext.SaveChanges();
                    return "Add Successfully.";
                }
                else
                {
                    return "Record Exists.";
                }
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
         
        }
        public Patient GetPatientById(int Id)
        {
            return GetPatients().FirstOrDefault(x => x.Id == Id);
        }

        public List<Patient> GetPatients()

        {
            List<Appointment> _app = GetAppointments();
            var _pat = from pat in _dbcontext.patients.ToList()
                       join gen in _dbcontext.genders.ToList() on pat.Gender equals gen.Id
                       select new Patient
                       {
                           Id = pat.Id,
                           PatientName = pat.PatientName,
                           Age = pat.Age,
                           Gender = pat.Gender,
                           Mobile = pat.Mobile,
                           Address = pat.Address,
                           GenderName = gen.Name,
                           List_Appointment = _app.FindAll(x => x.PatientId == pat.Id)
                       };
            return _pat.ToList();
        }
        #endregion
        #region Specialist and Gender
        public string AddSpecialist(Specialist _spec)
        {
            try
            {
                if (_dbcontext.specialists.FirstOrDefault(x => x.Name == _spec.Name) == null)
                {
                    _dbcontext.specialists.Add(_spec);
                    _dbcontext.SaveChanges();
                    return "Added Successfully";
                }
                else
                {
                    return "Record Exists.";
                }
            }catch(Exception ex)
            {
                _logger.LogError("AddSpecialist",ex);
                return ex.ToString();
            }
         
        }
        public List<Gender> GetGender()
        {
            List<Gender> _gender = new List<Gender>()
            {
                new Gender{Name="Male"},
                new Gender{Name="Female"},
                new Gender{Name="Bigender"}
            };
            foreach(var item in _gender)
            {
                if(_dbcontext.genders.FirstOrDefault(x=>x.Name == item.Name) == null)
                {
                    _dbcontext.genders.Add(item);
                    _dbcontext.SaveChanges();
                }
            }
            
            return _dbcontext.genders.ToList();
        }
        public Specialist GetSpecialistById(int Id)
        {
            return _dbcontext.specialists.FirstOrDefault(x => x.Id == Id);
        }
        public List<Specialist> GetSpecialists()
        {
            var _spec = from spe in _dbcontext.specialists.ToList()
                        join doc in _dbcontext.doctors.ToList() on spe.Id equals doc.SpecialistId into DocGroup
                        from d in DocGroup.DefaultIfEmpty() group d by spe into g
                        select new Specialist
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name,
                            Qualification = g.Key.Qualification,
                            List_doctor = _dbcontext.doctors.ToList().FindAll(x=>x.SpecialistId== g.Key.Id) , //g.Select(x => x.d).Where(x => x != null).ToList(),
                            DoctorCount = g.Count(d => d != null)  // Set DoctorCount to 1 if a doctor exists, 0 otherwise
                        };
            return _spec.ToList();
        }

        #endregion
        #region Appointment
        public string AddAppointment(Appointment _app)
        {
            try
            {
                if (_dbcontext.appointments.ToList().FindAll(x => x.PatientId == _app.PatientId && x.DoctorId == _app.DoctorId && x.VisiteDate.ToString("dd-MMM-yyyy") == _app.VisiteDate.ToString("dd-MMM-yyyy") && x.status == false).Count == 0)
                {
                    _dbcontext.appointments.Add(_app);
                    _dbcontext.SaveChanges();
                    return "Appointment added successfully";
                }
                else
                {
                    return "Already appointment added.";
                }
            }
           catch(Exception ex)
            {
                return ex.ToString();
            }
          
        }

        public List<Appointment> GetAppointments()
        {
            List<Appointment> _app = (from app in _dbcontext.appointments.ToList()
                                      join pat in _dbcontext.patients.ToList() on app.PatientId equals pat.Id
                                      join dot in _dbcontext.doctors.ToList() on app.DoctorId equals dot.Id
                                      select new Appointment
                                      {
                                          Id = app.Id,
                                          DoctorId = app.DoctorId,
                                          DoctorName = dot.DoctorName,
                                          PatientId = app.PatientId,
                                          PatientName = pat.PatientName,
                                          ReasonForVisite = app.ReasonForVisite,
                                          VisiteDate = app.VisiteDate,
                                          status = app.status
                                      }).ToList();
            return _app;
        }

       
        #endregion
    }
}
