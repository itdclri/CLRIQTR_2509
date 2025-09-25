using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLRIQTR.Models
{
    [Table("qtr_type_master")]
    public class QtrTypeMaster
    {
        [Key]
        public int qid { get; set; }

        [Required]
        [StringLength(120)]
        public string qtrdesc { get; set; }

        [Required]
        [StringLength(30)]
        public string qtrtype { get; set; }
    }
}