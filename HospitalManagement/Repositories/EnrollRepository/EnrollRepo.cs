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
        private readonly HMContextt _dbcontext;
        private readonly ILogger<EnrollRepo> _logger;
        public EnrollRepo(ILogger<EnrollRepo> logger)
        {
            _dbcontext = new HMContextt();
            _logger =logger;
            GetGender();
        }

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
                    doct.Address = _doctor.Address;
                    _dbcontext.SaveChanges();
                    return "Updated successfully.";
                }
                else
               if(_dbcontext.doctors.ToList().FirstOrDefault(x=>x.DoctorName == _doctor.DoctorName && x.SpecialistId == _doctor.SpecialistId) == null)
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

        public string AddPatient(Patient _patient)
        {
            if(_dbcontext.patients.FirstOrDefault(x=>x.PatientName==_patient.PatientName && x.Gender== _patient.Gender) == null)
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

        public Doctor GetDoctorByID(int Id)
        {
           return GetDoctors().FirstOrDefault(x=>x.Id ==Id);
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
                              Address = doc.Address,
                              SpecialistId = doc.SpecialistId,
                              SpecialistName = spe.Name
                          };
            return _doctor.ToList();
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
                }
            }
            _dbcontext.SaveChanges();
            return _dbcontext.genders.ToList();
        }

        public Patient GetPatientById(int Id)
        {
            return _dbcontext.patients.FirstOrDefault(x=>x.Id == Id);
        }

        public List<Patient> GetPatients()
        {
            return _dbcontext.patients.ToList();
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
                            DoctorCount = g.Count(d => d != null)  // Set DoctorCount to 1 if a doctor exists, 0 otherwise
                        };
            return _spec.ToList();
        }
    }
}
