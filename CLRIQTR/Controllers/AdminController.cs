using CLRIQTR.Data.Repositories.Implementations;
using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using CLRIQTR.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace CLRIQTR.Controllers
{
    public class AdminController : Controller
    {
        private readonly IEmployeeRepository _employeeRepo;
        private readonly IQuarterRepository _quarterRepo;
        private readonly ILookupRepository _lookupRepo;
        private readonly IQuarterService _quarterService;

        public AdminController()
        {
            _employeeRepo = new EmployeeRepository();
            _quarterRepo = new QuarterRepository();
            _lookupRepo = new LookupRepository();
            _quarterService = new QuarterService(_quarterRepo, _employeeRepo);
        }

        // =============================
        // MAIN EMPLOYEE LISTING PAGES
        // =============================
        public ActionResult Index(string EmpNo = null, string EmpName = null, string Designation = null, int page = 1, int pageSize = 10, string Status = null)
        {
            if (!int.TryParse(Session["LabCode"]?.ToString(), out int adminLabCode))
                return RedirectToAction("Index", "Login");

            var employees = _employeeRepo.GetEmployeesByLab(adminLabCode, EmpNo, EmpName, Designation, Status);
            var totalCount = employees.Count;
            var pagedEmployees = employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var labs = _lookupRepo.GetLabs();
            foreach (var emp in pagedEmployees)
            {
                emp.LabName = labs.FirstOrDefault(l => l.LabCode == emp.LabCode)?.LabName;
            }

            ViewBag.EmpNoFilter = EmpNo;
            ViewBag.EmpNameFilter = EmpName;
            ViewBag.DesignationFilter = Designation;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.StatusFilter = Status;

            var designations = _lookupRepo.GetDesignations()
                                 .Select(d => new { Value = d.DesId, Text = d.DesDesc })
                                 .ToList();
            ViewBag.DesignationList = new SelectList(designations, "Value", "Text", Designation);

            return View(pagedEmployees);
        }

        public ActionResult SERC(string EmpNo = null, string EmpName = null, string Designation = null, int page = 1, int pageSize = 10, string Status = null)
        {
            return GetLabEmployees(101, EmpNo, EmpName, Designation, page, pageSize, Status, "SERC");
        }

        public ActionResult CMC(string EmpNo = null, string EmpName = null, string Designation = null, int page = 1, int pageSize = 10, string Status = null)
        {
            return GetLabEmployees(102, EmpNo, EmpName, Designation, page, pageSize, Status, "CMC");
        }

        private ActionResult GetLabEmployees(int labCode, string empNo, string empName, string designation, int page, int pageSize, string status, string viewName)
        {
            if (!int.TryParse(Session["LabCode"]?.ToString(), out int adminLabCode))
                return RedirectToAction("Index", "Login");

            var employees = _employeeRepo.GetEmployeesByLab(labCode, empNo, empName, designation, status);
            var totalCount = employees.Count;
            var pagedEmployees = employees.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var labs = _lookupRepo.GetLabs();
            foreach (var emp in pagedEmployees)
            {
                emp.LabName = labs.FirstOrDefault(l => l.LabCode == emp.LabCode)?.LabName;
            }

            ViewBag.EmpNoFilter = empNo;
            ViewBag.EmpNameFilter = empName;
            ViewBag.DesignationFilter = designation;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.StatusFilter = status;

            var designations = _lookupRepo.GetDesignations()
                                 .Select(d => new { Value = d.DesId, Text = d.DesDesc })
                                 .ToList();
            ViewBag.DesignationList = new SelectList(designations, "Value", "Text", designation);

            return View(viewName, pagedEmployees);
        }

        // =============================
        // EMPLOYEE CRUD OPERATIONS
        // =============================
        private void LoadDropdowns(int? selectedLab = null, string selectedDesignation = null)
        {
            var labs = _lookupRepo.GetLabs()
                         .Select(l => new { Value = l.LabCode, Text = l.LabName })
                         .ToList();

            var designations = _lookupRepo.GetDesignations()
                                 .Select(d => new { Value = d.DesId, Text = d.DesDesc })
                                 .ToList();

            ViewBag.Labs = new SelectList(labs, "Value", "Text", selectedLab);
            ViewBag.Designations = new SelectList(designations, "Value", "Text", selectedDesignation);

            ViewBag.GenderList = new SelectList(new[]
            {
                new { Value = "M", Text = "Male" },
                new { Value = "F", Text = "Female" }
            }, "Value", "Text");

            ViewBag.ActiveList = new SelectList(new[]
            {
                new { Value = "Y", Text = "Yes" },
                new { Value = "N", Text = "No" }
            }, "Value", "Text");

            ViewBag.PhyList = new SelectList(new[]
            {
                new { Value = "Y", Text = "Yes" },
                new { Value = "N", Text = "No" }
            }, "Value", "Text");

            var basicPayOptions = new List<SelectListItem>();
            for (int i = 1; i <= 13; i++)
            {
                basicPayOptions.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            basicPayOptions.Add(new SelectListItem { Text = "13 A", Value = "131" });
            basicPayOptions.Add(new SelectListItem { Text = "14", Value = "14" });

            ViewBag.BasicPayList = new SelectList(basicPayOptions, "Value", "Text");
        }

        public ActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(EmpMastTest emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    emp.DOB = emp.DOB_dt?.ToString("dd-MM-yyyy");
                    emp.DOJ = emp.DOJ_dt?.ToString("dd-MM-yyyy");
                    emp.DOP = emp.DOP_dt?.ToString("dd-MM-yyyy");
                    emp.DOR = emp.DOR_dt?.ToString("dd-MM-yyyy");

                    emp.Gender = emp.Gender?.ToUpper() == "M" ? "M" : emp.Gender?.ToUpper() == "F" ? "F" : null;
                    emp.Active = "Y";
                    emp.Phy = emp.Phy?.ToUpper() == "Y" ? "Y" : "N";

                    emp.EnteredDate = DateTime.UtcNow;
                    emp.EnteredIP = Request.UserHostAddress;

                    _employeeRepo.AddEmployee(emp, Request.UserHostAddress);

                    TempData["Message"] = "Employee added successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                }
            }

            LoadDropdowns(emp.LabCode, emp.Designation);
            return View(emp);
        }

        public ActionResult Edit(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var emp = _employeeRepo.GetEmployeeByEmpNo(id);
            if (emp == null) return HttpNotFound();

            if (!string.IsNullOrEmpty(emp.DOB) && DateTime.TryParseExact(emp.DOB, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob))
                emp.DOB_dt = dob;

            if (!string.IsNullOrEmpty(emp.DOJ) && DateTime.TryParseExact(emp.DOJ, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime doj))
                emp.DOJ_dt = doj;

            if (!string.IsNullOrEmpty(emp.DOP) && DateTime.TryParseExact(emp.DOP, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dop))
                emp.DOP_dt = dop;

            if (!string.IsNullOrEmpty(emp.DOR) && DateTime.TryParseExact(emp.DOR, "dd-MM-yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dor))
                emp.DOR_dt = dor;

            LoadDropdowns(emp.LabCode, emp.Designation);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(EmpMastTest emp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    emp.DOB = emp.DOB_dt?.ToString("dd-MM-yyyy");
                    emp.DOJ = emp.DOJ_dt?.ToString("dd-MM-yyyy");
                    emp.DOP = emp.DOP_dt?.ToString("dd-MM-yyyy");
                    emp.DOR = emp.DOR_dt?.ToString("dd-MM-yyyy");

                    emp.Gender = emp.Gender?.ToUpper() == "M" ? "M" : emp.Gender?.ToUpper() == "F" ? "F" : null;
                    emp.Active = emp.Active?.ToUpper() == "Y" ? "Y" : "N";
                    emp.Phy = emp.Phy?.ToUpper() == "Y" ? "Y" : "N";

                    _employeeRepo.UpdateEmployee(emp);

                    TempData["Message"] = "Employee updated successfully!";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating: " + ex.Message);
                }
            }

            LoadDropdowns(emp.LabCode, emp.Designation);
            return View(emp);
        }

        public ActionResult Deactivate(string id)
        {
            if (id == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var emp = _employeeRepo.GetEmployeeByEmpNo(id);
            if (emp == null) return HttpNotFound();
            return View(emp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeactivateConfirmed(string id)
        {
            _employeeRepo.DeactivateEmployee(id);
            TempData["Message"] = "Employee deactivated successfully!";
            return RedirectToAction("Index");
        }

        // =============================
        // QUARTER MANAGEMENT (NEW IMPLEMENTATION)
        // =============================
        public ActionResult QuarterDetails(string empNo)
        {
            var model = _quarterService.GetQuarterDetails(empNo);

            
            var quarterTypes = _quarterService.GetQuarterTypes(); 
            if (model == null)
            {
                ViewBag.QtrTypes = new SelectList(quarterTypes, "Code", "Description");
                return View("InsertQuarterDetails", new QtrUpd { empno = empNo });
            }

            ViewBag.QtrTypes = new SelectList(quarterTypes, "Code", "Description", model.qtrtype);
            return View("UpdateQuarterDetails", model);
        }


        [HttpGet]
        public ActionResult UpdateQuarterDetails(string empNo)
        {
            return QuarterDetails(empNo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateQuarterDetails(QtrUpd model, string[] selectedParts)
        {
            Debug.WriteLine("Update - V");
            return ProcessQuarterForm(model, selectedParts, true) ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InsertQuarterDetails(QtrUpd model, string[] selectedParts)
        {
            return ProcessQuarterForm(model, selectedParts, false);
        }

        private ActionResult ProcessQuarterForm(QtrUpd model, string[] selectedParts, bool isUpdate)
        {

            Debug.WriteLine("Update - V1");

            if (ModelState.IsValid)
            {
                var result = _quarterService.InsertOrUpdateQuarter(model, selectedParts, isUpdate);

                if (result.Success)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction("UpdateQuarterDetails", new { empNo = result.RedirectEmpNo });
                }

                ModelState.AddModelError("", result.Message);
            }

            ViewBag.QtrTypes = new SelectList(_quarterService.GetQuarterTypes(), "Code", "Description", model.qtrtype);
            return View(isUpdate ? "UpdateQuarterDetails" : "InsertQuarterDetails", model);
        }

        // =============================
        // AJAX METHODS
        // =============================
        public JsonResult GetPartsByQtr(string qtrType, string empNo = null)
        {
            try
            {
                if (string.IsNullOrEmpty(qtrType))
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);

                var quarterTypes = _quarterRepo.GetAllQuarterTypes();
                var quarterType = quarterTypes.FirstOrDefault(q => q.qtrtype == qtrType);

                if (quarterType == null)
                    return Json(new List<object>(), JsonRequestBehavior.AllowGet);

                var parts = _quarterRepo.GetPartsWithOccupancyByDesc(quarterType.qtrdesc, empNo)
                              .Select(p => new {
                                  Value = p.PartNumber,
                                  Text = p.PartNumber,
                                  Occupied = p.Occupied,
                                  OccupiedBy = p.OccupiedBy,
                                  IsCurrentUser = p.IsCurrentUser
                              })
                              .ToList();

                return Json(parts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckPartOccupancy(string part)
        {
            try
            {
                var conflict = _quarterRepo.GetQuarterByPart(part);
                if (conflict != null)
                {
                    return Json(new
                    {
                        occupied = true,
                        occupiedBy = conflict.empno,
                        currentUser = false
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    occupied = false,
                    occupiedBy = (string)null,
                    currentUser = false
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    occupied = false,
                    occupiedBy = (string)null,
                    currentUser = false,
                    error = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetPartsFast(string qtrtype, string currentEmpNo = null)
        {
            try
            {
                var parts = _quarterRepo.GetPartsWithOccupancy(qtrtype, currentEmpNo);
                return Json(parts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult ViewEmployeeDetails(string empNo)
        {
            if (string.IsNullOrWhiteSpace(empNo))
                return new HttpStatusCodeResult(400, "Employee number is required.");

            var emp = _employeeRepo.GetEmployeeByNoForView(empNo);
            if (emp == null)
                return HttpNotFound("Employee not found.");

            var labs = _lookupRepo.GetLabs();
            emp.LabName = labs.FirstOrDefault(l => l.LabCode == emp.LabCode)?.LabName;

            return PartialView("_EmployeeDetailsPartial", emp);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        // =============================
        // NEW QUARTER SERVICE METHODS
        // =============================
        public JsonResult GetPartsByQtrDesc(string qtrDesc, string empNo = null)
        {
            try
            {
                var parts = _quarterService.GetPartsByQuarterDesc(qtrDesc, empNo)
                              .Select(p => new {
                                  Value = p.PartNumber,
                                  Text = p.PartNumber,
                                  Occupied = p.Occupied,
                                  OccupiedBy = p.OccupiedBy,
                                  IsCurrentUser = p.IsCurrentUser
                              })
                              .ToList();

                return Json(parts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckQtrAvailability(string qtrNo, string empNo = null)
        {
            try
            {
                bool isAvailable = _quarterService.ValidateQuarterAvailability(qtrNo, empNo);
                return Json(new { available = isAvailable }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}