using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CLRIQTR.Services
{
    public interface IQuarterService
    {
        QuarterOperationResult InsertOrUpdateQuarter(QtrUpd model, string[] selectedParts, bool isUpdate);
        QtrUpd GetQuarterDetails(string empNo);
        List<QuarterType> GetQuarterTypes();
        List<QuarterPart> GetPartsByQuarterDesc(string qtrDesc, string currentEmpNo = null);
        bool ValidateQuarterAvailability(string qtrNo, string currentEmpNo = null);
    }

    public class QuarterService : IQuarterService
    {
        private readonly IQuarterRepository _quarterRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public QuarterService(IQuarterRepository quarterRepository, IEmployeeRepository employeeRepository)
        {
            _quarterRepository = quarterRepository;
            _employeeRepository = employeeRepository;
        }

        public QuarterOperationResult InsertOrUpdateQuarter(QtrUpd model, string[] selectedParts, bool isUpdate)
        {
            Debug.WriteLine("Update - V3");


            var result = new QuarterOperationResult { Success = false };

            try
            {
                // Get quarter description from type
                var quarterTypes = _quarterRepository.GetAllQuarterTypes();
                var quarterType = quarterTypes.FirstOrDefault(q => q.qtrtype == model.qtrtype);

                

                    if (quarterType == null)
                    {
                        result.Message = "Invalid quarter type selected.";
                        return result;
                    }

                    // Build the qtrno
                    string qtrNo = BuildQtrNo(quarterType.qtrdesc, selectedParts);

               



                    // Check if qtrno is already occupied by another employee
                    if (!isUpdate || (isUpdate && !IsSameEmployeeQuarter(qtrNo, model.empno)))
                    {
                        Debug.WriteLine("Update - V4");


                        if (!_quarterRepository.IsQtrNoAvailable(qtrNo, model.empno))
                        {
                            result.Message = $"Quarter {qtrNo} is already occupied by another employee.";
                            return result;
                        }
                    }

                    model.qtroldno = model.qtrno;
                    SetQuarterFields(model, quarterType, selectedParts, qtrNo);

                    // Set all quarter fields


                    // Get employee lab code for new records
                    if (!isUpdate)
                    {
                        var emp = _employeeRepository.GetEmployeeByEmpNo(model.empno);
                        if (emp == null)
                        {
                            result.Message = "Employee not found.";
                            return result;
                        }
                        model.labcode = emp.LabCode.ToString();
                        model.occdate = model.occdate ?? DateTime.Now;
                    }


                    


                    // Perform insert or update
                    if (isUpdate)
                    {
                        Debug.WriteLine((model.qtrno));

                        _quarterRepository.UpdateQuarterStatus(model);
                        result.Message = "Quarter details updated successfully!";
                    }
                    else
                    {
                        _quarterRepository.InsertQuarter(model);
                        result.Message = "Quarter details inserted successfully!";
                    }


                if (!string.IsNullOrEmpty(model.qtroldno))
                {
                    // Compare with current quarter number
                    if (model.qtroldno == model.qtrno)
                    {
                        isUpdate = true;
                        _quarterRepository.UpdateQuarterStatus(model);
                    }
                    else
                    {
                        // Vacate the existing quarter first
                        _quarterRepository.UpdateVacant(model);

                        // Then insert new quarter assignment
                        _quarterRepository.InsertQuarter(model);
                    }
                }
                else
                {
                    // No previous quarter - direct insert
                    _quarterRepository.InsertQuarter(model);
                }


                result.Success = true;
                    result.RedirectEmpNo = model.empno;

              
            }
            catch (Exception ex)
            {
                result.Message = $"An error occurred: {ex.Message}";
            }

            return result;
        }

        private string BuildQtrNo(string qtrDesc, string[] selectedParts)
        {
            if (selectedParts == null || selectedParts.Length == 0)
                return qtrDesc;

            var parts = selectedParts.OrderBy(p => int.TryParse(p, out var n) ? n : int.MaxValue).ToList();

            switch (parts.Count)
            {
                case 1:
                    return $"{qtrDesc}-{parts[0]}";
                case 2:
                    return $"{qtrDesc}-{parts[0]} & {parts[1]}";
                case 3:
                    return $"{qtrDesc}-{parts[0]} & {parts[1]}, {parts[2]}";
                default:
                    // For more than 3, fallback to default join with &
                    return $"{qtrDesc}-{string.Join(" & ", parts)}";
            }
        }


        private bool IsSameEmployeeQuarter(string qtrNo, string empNo)
        {
            var existingQuarter = _quarterRepository.GetQuarterByQtrNo(qtrNo);
            return existingQuarter != null && existingQuarter.empno == empNo;
        }

        private void SetQuarterFields(QtrUpd model, QtrTypeMaster quarterType, string[] selectedParts, string qtrNo)
        {
            model.qtrno = qtrNo;
            model.qtrno1 = quarterType.qtrdesc;

            if (selectedParts != null && selectedParts.Any())
            {
                model.qtrno2 = selectedParts.Length > 0 ? selectedParts[0] : null;
                model.qtrno3 = selectedParts.Length > 1 ? selectedParts[1] : null;
                model.qtr_count = selectedParts.Length;
            }
            else
            {
                model.qtrno2 = null;
                model.qtrno3 = null;
                model.qtr_count = 0;
            }
        }

        public QtrUpd GetQuarterDetails(string empNo)
        {
            var quarter = _quarterRepository.GetQuarterByEmployee(empNo);
            return quarter; // Return the QtrUpd object directly
       
        }

        public List<QuarterType> GetQuarterTypes()
        {
            return _quarterRepository.GetAllQuarterTypes()
                .Select(q => new QuarterType
                {
                    Id = q.qid,
                    Code = q.qtrtype,
                    Description = q.qtrdesc
                })
                .ToList();
        }

        public List<QuarterPart> GetPartsByQuarterDesc(string qtrDesc, string currentEmpNo = null)
        {
            var parts = _quarterRepository.GetPartsWithOccupancyByDesc(qtrDesc, currentEmpNo);

            return parts.Select(p => new QuarterPart
            {
                PartNumber = p.PartNumber,
                Occupied = p.Occupied,
                OccupiedBy = p.OccupiedBy,
                IsCurrentUser = p.IsCurrentUser
            }).ToList();
        }

        public bool ValidateQuarterAvailability(string qtrNo, string currentEmpNo = null)
        {
            return _quarterRepository.IsQtrNoAvailable(qtrNo, currentEmpNo);
        }
    }

    public class QuarterOperationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string RedirectEmpNo { get; set; }
    }

    public class QuarterViewModel
    {
        public string QtrNo { get; set; }
        public string EmpNo { get; set; }
        public string Status { get; set; }
        public string LabCode { get; set; }
        public DateTime? OccDate { get; set; }
        public string QtrType { get; set; }
        public string Remarks { get; set; }
        public string FNAN { get; set; }
        public string QtrNo1 { get; set; }
        public string QtrNo2 { get; set; }
        public string QtrNo3 { get; set; }
        public int? QtrCount { get; set; }
    }

    public class QuarterType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class QuarterPart
    {
        public string PartNumber { get; set; }
        public bool Occupied { get; set; }
        public string OccupiedBy { get; set; }
        public bool IsCurrentUser { get; set; }
    }
}