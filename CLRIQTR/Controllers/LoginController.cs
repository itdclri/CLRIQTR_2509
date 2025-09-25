using CLRIQTR.Data.Repositories.Implementations;
using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CLRIQTR.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepo;
        private readonly ILookupRepository _lookupRepo;

        public LoginController()
        {
            _loginRepo = new LoginRepository();
            _lookupRepo = new LookupRepository();
        }

        // GET: Login
        public ActionResult Index()
        {
            LoadLabs();
            return View();
        }

        [HttpPost]
        public ActionResult Index(string EmpNo, string password, int? LabCode)
        {
            if (string.IsNullOrEmpty(EmpNo) || LabCode == null || LabCode == 0)
            {
                ViewBag.Error = "Please enter Employee No and select a Lab.";
                LoadLabs();
                return View();
            }

            var emp = _loginRepo.ValidateLogin(EmpNo, password, LabCode.Value);

            if (emp != null)
            {
                Session["LabCode"] = emp.LabCode;
                Session["EmpNo"] = emp.EmpNo;
                Session["Designation"] = emp.Designation;

                return RedirectToAction("Index", "Admin");
            }

            ViewBag.Error = "Invalid Employee No, Lab, or account inactive.";
            LoadLabs();
            return View();
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index");
        }

        private void LoadLabs()
        {
            // USING RAW SQL REPOSITORY
            var labs = _lookupRepo.GetLabs()
                .Select(l => new { Value = l.LabCode, Text = l.LabName })
                .ToList();

            ViewBag.Labs = new SelectList(labs, "Value", "Text");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    _loginRepo?.Dispose();
        //    _lookupRepo?.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}