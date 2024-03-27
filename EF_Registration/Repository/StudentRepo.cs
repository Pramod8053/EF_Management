using EF_Registration.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EF_Registration.Repository
{
    public class StudentRepo : IStudentRepo
    {
        private readonly DBContextStudente _dbContext;
        public StudentRepo()
        {
            _dbContext = new DBContextStudente();
        }
        public int DeleteStudent(int StudentID)
        {
            try
            {
                Student std = _dbContext.students.FirstOrDefault(x=>x.StudentID==StudentID);
                _dbContext.students.Remove(std);
                _dbContext.SaveChanges();
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public Student GetByIDStudent(int StudentID)
        {
            return _dbContext.students.FirstOrDefault(x=>x.StudentID==StudentID);
        }

        public List<District> GetDistrict()
        {
            List<District> distlst = new List<District>() { 
            new District{DistrictID=1,StateListID=1,DistrictName="Khorda"},
            new District{DistrictID=2,StateListID=1,DistrictName="Cuttack"},
            new District{DistrictID=3,StateListID=1,DistrictName="Jajpur"},
            new District{DistrictID=4,StateListID=1,DistrictName="Sambalpur"},
            new District{DistrictID=5,StateListID=1,DistrictName="Bhadrak"},
            new District{DistrictID=6,StateListID=2,DistrictName="North Goa"},
            new District{DistrictID=7,StateListID=2,DistrictName="South Goa"},
            new District{DistrictID=8,StateListID=3,DistrictName="Araria"},
            new District{DistrictID=9,StateListID=3,DistrictName="Banka"},
            new District{DistrictID=10,StateListID=3,DistrictName="Bhagalpur"},
            new District{DistrictID=11,StateListID=3,DistrictName="Bhojpur"},
            new District{DistrictID=12,StateListID=3,DistrictName="Gaya"},
            };
           
            return distlst;
        }

        public List<StateList> GetStateList()
        {
            List<StateList> statelst = new List<StateList>() {
                new StateList{StateListID=1,StateName="Odisha"},
                new StateList{StateListID=2,StateName="Goa"},
                new StateList{StateListID=3,StateName="Bihar"}
            };
            return statelst;
        }

        public List<Student> GetStudent()
        {
            var ds = from st in _dbContext.students.ToList()
                     join sl in GetStateList() on st.StateListID equals sl.StateListID
                     join dl in GetDistrict() on st.DistrictID equals dl.DistrictID
                  select new Student { 
                  StudentID = st.StudentID,
                  Name =st.Name,
                  Age=st.Age,
                  EmailID=st.EmailID,
                  MobileNo=st.MobileNo,
                  Address =st.Address,
                  StateName=sl.StateName,
                  DistrictName=dl.DistrictName,
                  PassportImg=st.PassportImg 
                  };

            return ds.ToList();
        }

        public int ModifyStudent(Student std)
        {
            try
            {
                Student _std = _dbContext.students.FirstOrDefault(x => x.StudentID == std.StudentID);
                if (_std != null)
                {
                    _std.Name = std.Name;
                    _std.Age = std.Age;
                    _std.Address = std.Address;
                    _std.MobileNo = std.MobileNo;
                    _std.EmailID = std.EmailID;
                    _std.DistrictID = std.DistrictID;
                    _std.StateListID = std.StateListID;
                    _std.PassportImg = std.PassportImg;
                    _std.StudentImg = std.StudentImg;
                    _dbContext.SaveChanges();
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
         catch(Exception ex)
            {
                throw ex;
            }
            return 0;
        }

        public int SaveStudent(Student std)
        {
            try
            {
                _dbContext.students.Add(std);
                _dbContext.SaveChanges();
                return 1;
            }
          catch(Exception ex)
            {
                throw ex;
               
            }
            return 0;
        }
    }
}
