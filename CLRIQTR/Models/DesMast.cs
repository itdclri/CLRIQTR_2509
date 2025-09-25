using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CLRIQTR.Models
{

    [Table("desmast")]
    public class DesMast
    {
        [Key]
        public int DesId { get; set; }

        [StringLength(245)]
        public string DesDesc { get; set; }
    }
}