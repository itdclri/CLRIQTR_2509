using CLRIQTR.Models;
using System.Collections.Generic;

namespace CLRIQTR.Data.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        // RAW SQL approach (PRIMARY)
        EmpMastTest ValidateLogin(string empNo, string password, int labCode);
        List<LabMast> GetAllLabs();

        // LINQ approach (ALTERNATIVE)
        // EmpMastTest ValidateLogin_LINQ(string empNo, int labCode);
    }
}