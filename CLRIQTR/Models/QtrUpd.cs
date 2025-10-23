using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLRIQTR.Models
{
    [Table("qtrupd")]
    public class QtrUpd
    {
        public string qtrno { get; set; }

        [Key]
        public string empno { get; set; }
        public string qtrstatus { get; set; }
        public string labcode { get; set; }
        public DateTime? occdate { get; set; }
        public string qtrtype { get; set; }
        public string rem { get; set; }
        public string restype { get; set; }
        public string resname { get; set; }
        public string qtrno1 { get; set; }
        public string qtrno2 { get; set; }
        public string qtrno3 { get; set; }
        public int? qtr_count { get; set; }
        public string FNAN { get; set; }

        [NotMapped]
        public string[] selectedParts { get; set; }

        [NotMapped]
        public string qtrdesc { get; set; }

        [NotMapped]
        public string qtroldno { get; set; }

        public string part { get; set; }

    }

    public class RoomHistoryViewModel
    {
        public string EmpNo { get; set; }
        public string EmpName { get; set; }
        public string DateOfOccupancy { get; set; }
        public string DateOfVacation { get; set; }
    }
}