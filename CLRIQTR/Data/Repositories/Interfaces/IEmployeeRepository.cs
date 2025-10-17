using CLRIQTR.Models;
using System.Collections.Generic;
using ZstdSharp.Unsafe;

namespace CLRIQTR.Data.Repositories.Interfaces
{
    public interface IEmployeeRepository
    {
        // RAW SQL approach (PRIMARY)
        EmpMastTest GetEmployeeByEmpNo(string empNo);

        List<EmpMastTest> GetEmployeesByLab(int labCode, string empNo = null, string empName = null, string designation = null, string status = null);
        EmpMastTest GetEmployeeByNoForView(string empNo);

        void AddEmployee(EmpMastTest employee, string enteredIp);
        void UpdateEmployee(EmpMastTest employee);
        void DeactivateEmployee(string empNo);

        void AddDependent(EmpDependentDetail dependent);

        void UpsertFamilyDetails(EmpDepDtls details);

        // LINQ approach (ALTERNATIVE - commented but available)
        // EmpMastTest GetEmployeeByEmpNo_LINQ(string empNo);

        IEnumerable<DependentTypeDto> GetAllDependentTypes();

 
        List<DependentInputModel> GetDependentsByEmpNo(string empNo);
        void UpdateDependents(string empNo, List<DependentInputModel> dependents);

        DependentInputModel AddDependent(DependentInputModel dependent);
        void UpdateDependent(DependentInputModel dependent);
        void DeleteDependent(int id);

        IEnumerable<CompletedApplicationViewModel> GetAllCompletedApplications();

    }
}