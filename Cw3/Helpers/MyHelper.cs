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
    }
}
