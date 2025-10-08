using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CLRIQTR.Models
{
    [Table("empmast")]
    public class EmpMastTest
    {
        [Key]
        [Required(ErrorMessage = "Employee No is required")]
        [StringLength(45)]
        public string EmpNo { get; set; }

        [Required(ErrorMessage = "Employee Name is required")]
        [StringLength(45)]
        public string EmpName { get; set; }

        [Required(ErrorMessage = "Lab Code is required")]
        public int LabCode { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [StringLength(45)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Pay Level is required")]
        [StringLength(45)]
        public string PayLvl { get; set; }

        [Required(ErrorMessage = "Designation is required")]
        public string Designation { get; set; }

        [StringLength(45)]
        public string DOB { get; set; }

        [StringLength(45)]
        public string DOJ { get; set; }

        [Required(ErrorMessage = "Basic Pay is required")]
        public int? BasicPay { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [StringLength(45)]
        public string Category { get; set; }

        [StringLength(45)]
        public string DOP { get; set; }

        [StringLength(45)]
        public string DOR { get; set; }

        [StringLength(45)]
        public string PWD { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        [StringLength(145)]
        public string Email { get; set; }

        [StringLength(145)]
        public string EmpGroup { get; set; }

        [StringLength(145)]
        public string Grade { get; set; }

        [StringLength(45)]
        public string Active { get; set; }

        [Required(ErrorMessage = "Physical Handicap status is required")]
        [StringLength(45)]
        public string Phy { get; set; }

        [StringLength(45)]
        public string Checked { get; set; }

        [StringLength(45)]
        public string ChkDtte { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [Phone(ErrorMessage = "Invalid Mobile Number")]
        [StringLength(45)]
        public string MobileNumber { get; set; }

        [NotMapped]
        public string LabName { get; set; }

        [NotMapped]
        public string DesDesc { get; set; }

        public DateTime? EnteredDate { get; set; }
        public string EnteredIP { get; set; }

        //public string CatNew { get; set; }


        // ========== DateTime Properties for View ==========
        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOB_dt { get; set; }

        [Required(ErrorMessage = "Date of Joining is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOJ_dt { get; set; }

        [Required(ErrorMessage = "Date of Present Post is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOP_dt { get; set; }

        [Required(ErrorMessage = "Date of Superannuation is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DOR_dt { get; set; }


        [NotMapped]
        public string QtrNo { get; set; }

        [NotMapped]
        public string OccDate { get; set; }

        [NotMapped]
        public string QtrType { get; set; }

        [NotMapped]
        public string QtrStatus { get; set; }

        [NotMapped]
        public string Password { get; set; }


        public string FamilyStatus { get; set; }

        public List<DependentInputModel> Dependents { get; set; }

        public FamilyDetails FamilyDetails { get; set; }

    }

    // In ~/Models/EmpDepDtls.cs
    public class EmpDepDtls
    {
        public string EmpNo { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string WifeName { get; set; }
        public string HusbandName { get; set; }
        public string Son1 { get; set; }
        public string Son2 { get; set; }
        public string Son3 { get; set; }
        public string Daughter1 { get; set; }
        public string Daughter2 { get; set; }
        public string Daughter3 { get; set; }
    }


    public class EmpDepMast
    {
        public int depid { get; set; }
        public string depdesc { get; set; }
        public string depcode { get; set; }
    }

    public class DependentTypeDto
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
    }

    public class DependentInputModel
    {
        public int DependentTypeId { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public string EmpNo { get; set; }


    }


    public class EmpDependentDetail
    {
        public string EmpNo { get; set; }
        public int DepId { get; set; }
        public string DepName { get; set; }
    }

    public class FamilyDetails
    {
        
        public string FamilyStatus { get; set; }

       
    }


}
