using CLRIQTR.Models;
using System;
using System.Collections.Generic;

namespace CLRIQTR.Data.Repositories.Interfaces
{
    public interface IQuarterRepository : IDisposable
    {
        // Quarter Master Data Operations
        List<QtrTypeMaster> GetAllQuarterTypes();
        
        List<QtrMaster> GetQuarterDetailsByType(string qtrType);

        // Quarter Assignment Operations (CRUD)
        QtrUpd GetQuarterByEmployee(string empNo);
        QtrUpd GetQuarterByQtrNo(string qtrNo);
        QtrUpd GetQuarterByPart(string part);
        void UpdateQuarterStatus(QtrUpd quarter);
        void UpdateVacant(QtrUpd quarter);
        void InsertQuarter(QtrUpd quarter);

        void InsertQtrTxn(QtrUpd quarter);

        // Quarter Availability Checking
        bool IsQtrNoAvailable(string qtrNo, string currentEmpNo = null);

        // Occupancy Information
        List<dynamic> GetPartsWithOccupancy(string qtrtype, string currentEmpNo);
        List<dynamic> GetPartsWithOccupancyByDesc(string qtrdesc, string currentEmpNo);

        // IDisposable Implementation
        void Dispose();
    }
}