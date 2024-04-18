using HospitalManagement.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HospitalManagement.Repositories.EnrollRepository
{
   public interface IEnrollRepo
    {
        List<Gender> GetGender();
        List<Specialist> GetSpecialists();
        Specialist GetSpecialistById(int Id);
        string AddSpecialist(Specialist _spec);
        List<Patient> GetPatients();
        
        Patient GetPatientById(int Id);
        string AddPatient(Patient _patient);
        List<Doctor> GetDoctors();
        Doctor GetDoctorByID(int Id);
        string AddDoctor(Doctor _doctor);
        string AddAppointment(Appointment _app);
        List<Appointment> GetAppointments();
    }
}
