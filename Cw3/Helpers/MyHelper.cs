using Cw3.Models;
using Cw3.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Helpers
{
    public class MyHelper
    {
        public MyHelper(string message, int value)
        {
            this.message = message;
            this.value = value;
        }

        public string message { get; set; }
        public int value { get; set; }
        /*public EnrollmentResponse enrollment { get; set; }*/
        public Enrollment enrollment { get; set; }
    }
}
