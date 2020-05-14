using System;
using System.Collections.Generic;

namespace Cw3.Models
{
    public partial class Student
    {
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public int IdEnrollment { get; set; }
        public string Haslo { get; set; }
        public string Salt { get; set; }
        public string ReToken { get; set; }

        public virtual Enrollment IdEnrollmentNavigation { get; set; }
    }
}
