using System.Collections.Generic;

namespace CLRIQTR.Models
{
    public class TentativeReportModel
    {
        public int SNo { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string LabInstitute { get; set; }
        public string DateOfJoining { get; set; }
        public string Remarks { get; set; }
        public string DateOfBirth { get; set; }
        public string PayLevel { get; set; }
        public string PriorityDate { get; set; }
        public string QuarterType { get; set; }
        public string Category { get; set; }
        public string OwnHouse { get; set; }
    }

    public class TentativeReportViewModel
    {
        public List<TentativeReportModel> TypeI { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeII { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIISC { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIII { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIIISC { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIIIST { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIV { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIVSC { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeIVST { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeV { get; set; } = new List<TentativeReportModel>();
        public List<TentativeReportModel> TypeSA { get; set; } = new List<TentativeReportModel>();
    }
}