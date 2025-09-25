using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLRIQTR.Models
{
    [Table("qtr_master")]
    public class QtrMaster
    {
        [Key]
        public int qtr_id { get; set; }

        [StringLength(120)]
        public string qtr_desc { get; set; }

        [StringLength(40)]
        public string qtr_no { get; set; }

        [StringLength(30)]
        public string qtr_type { get; set; }

        [StringLength(200)]
        public string qtr_full_label { get; set; }
    }
}