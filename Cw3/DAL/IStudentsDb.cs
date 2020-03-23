using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.DAL
{
    public interface IStudentsDb
    {
        public IEnumerable<Student> GetStudents();
        public void AddStudent(Student student);
        public StundetEnrollment GetStudyInfo(string id);
        public Student GetStudent(string id);

    }
}
