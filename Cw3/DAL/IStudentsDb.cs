using Cw3.Helpers;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public interface IStudentsDb
    {
        public IEnumerable<Student> GetStudents();
        public MyHelper AddStudent(Student student);
        public StundetEnrollment GetStudyInfo(string id);
        public Student GetStudent(string id);
        public MyHelper Promote(StundetEnrollment se);

    }
}
