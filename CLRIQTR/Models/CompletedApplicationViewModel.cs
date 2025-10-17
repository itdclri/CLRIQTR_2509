using System;
using System.ComponentModel.DataAnnotations;

namespace CLRIQTR.Models 
{
    public class CompletedApplicationViewModel
    {
        [Display(Name = "Application No(s)")]
        public string qtrappno { get; set; }

        [Display(Name = "Employee No")]
        public string empno { get; set; }

        [Display(Name = "Employee Name")]
        public string empname { get; set; }

        [Display(Name = "Designation")]
        public string desdesc { get; set; }

        [Display(Name = "Date of Application")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? doa { get; set; }

        [Display(Name = "Status")]
        public string appstatus { get; set; }
    }
}