using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLRIQTR.Models
{

    [Table("labmast")]
    public class LabMast
    {
        [Key]
        public int LabCode { get; set; }

        [Required]
        [StringLength(45)]
        public string LabName { get; set; }
    }
}