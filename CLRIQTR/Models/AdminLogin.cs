using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CLRIQTR.Models
{
    public class AdminLogin
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string LabCode { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }

        public string Remarks  { get; set; }
}

}