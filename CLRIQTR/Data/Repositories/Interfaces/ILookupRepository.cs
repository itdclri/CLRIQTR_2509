using CLRIQTR.Models;
using System.Collections.Generic;

namespace CLRIQTR.Data.Repositories.Interfaces
{
    public interface ILookupRepository
    {
        // RAW SQL approach (PRIMARY)
        List<LabMast> GetLabs();
        List<DesMast> GetDesignations();

        // LINQ approach (ALTERNATIVE)
        // List<LabMast> GetLabs_LINQ();
    }
}