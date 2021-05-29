using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Sql;
using System.Data;
using System.Data.Entity;
using eResident_1._0.Models;
using System.Net;

namespace eResident_1._0.Controllers
{
    public class HomeController : Controller
    {
        private eResidentEntities db = new eResidentEntities();
        public ActionResult Index()
        {
            Session["CurrentUsername"] = null;
            Session["CurrentUserID"] = null;
            return View();
        }

        public ActionResult StaffLogin()
        {
            ViewBag.Message = TempData["checkrole"];

            return View();
        }

        public ActionResult ResidentLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StaffLogin(STAFF staff)
        {
            var UserCheck = db.STAFFs.Where(x => x.STAFF_EMAIL == staff.STAFF_EMAIL && x.PASSWORD == staff.PASSWORD).FirstOrDefault();

            if (UserCheck != null)
            {
                if (UserCheck.ROLE_ID == 1)
                    TempData["checkrole"] = "ROLE 1 STAFF";
                else if (UserCheck.ROLE_ID == 2)
                    TempData["checkrole"] = "ROLE 2 GUARD";
                Session["CurrentUsername"] = UserCheck.STAFF_NAME;
                Session["CurrentUserID"] = UserCheck.STAFF_ID;
                return RedirectToAction("Index", "Staff");
                
            }
            else
            {
                TempData["msg"] = "<script>alert('Invalid Email or Password. Please try again');</script>";
                return RedirectToAction("StaffLogin", "Home");
            }
        }
        [HttpPost]
        public ActionResult ResidentLogin(RESIDENT resident)
        {
            var UserCheck = db.RESIDENTs.Where(x => x.RESIDENT_IC == resident.RESIDENT_IC && x.PASSWORD == resident.PASSWORD).FirstOrDefault();

            if (UserCheck != null)
            {
                Session["CurrentUsername"] = UserCheck.RESIDENT_NAME;
                Session["CurrentUserID"] = UserCheck.RESIDENT_IC;
                return RedirectToAction("Index", "Resident");
            }
            else
            {
                TempData["msg"] = "<script>alert('Invalid Email or Password. Please try again');</script>";
                return RedirectToAction("ResidentLogin", "Home");
            }
        }
    }
}