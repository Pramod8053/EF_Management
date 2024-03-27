using EF_Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Registration.Repository
{
   public interface IStudentRepo
    {
        List<StateList> GetStateList();
        List<District> GetDistrict();
        List<Student> GetStudent();
        Student GetByIDStudent(int StudentID);
        int SaveStudent(Student std);
        int ModifyStudent(Student std);
        int DeleteStudent(int StudentID);
    }
}
