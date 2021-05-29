using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eResident_1._0.Models;
using System.Data;
using System.Data.Entity;
using System.Net;

namespace eResident_1._0.Controllers
{
    public class ResidentController : Controller
    {
        eResidentEntities db = new eResidentEntities();
        // GET: Resident
        public ActionResult Index()
        {
            string userID = Session["CurrentUserID"].ToString();
            string username = Session["CurrentUsername"].ToString();
            return View();
        }
        [HttpGet]
        public ActionResult BookService()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyBooking(BOOKING bookdetails)
        {
            try
            {
                string userID = Session["CurrentUserID"].ToString();
                db.PROC_InsertBooking(bookdetails.BOOKING_DATE, bookdetails.BOOKING_TIME, bookdetails.BOOKING_DESC, userID, bookdetails.SERVICE_ID);
                TempData["Success"] = true;
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
                TempData["ErrorMessage"] = ex;
            }

            return RedirectToAction("BookService", "Resident");
        }
        public ActionResult ViewBookingList()
        {
            string userID = Session["CurrentUserID"].ToString();
            var bookinglist = db.View_ResidentBookingList.Where(x => x.RESIDENT_IC == userID).OrderByDescending(X => X.REQ_ON);
            return View(bookinglist.ToList());
        }

        [HttpGet]
        public ActionResult ViewVisitor(DateTime? datevisit, string icnumber)
        {
            string userID = Session["CurrentUserID"].ToString();
            var visitor = db.VISITORs.Include(i => i.RESIDENT).Include(i => i.STAFF).Where(i => i.VISITOR_IC == icnumber || icnumber == "").Where(i => i.ARRIVAL_DATE == datevisit || datevisit == null).Where(i => i.RESIDENT_IC == userID);
            return View(visitor);
        }

        public ActionResult DeleteVisitor(int id)
        {
            db.VISITORs.Remove(db.VISITORs.Find(id));
            db.SaveChanges();
            return RedirectToAction("ViewVisitor");
        }

        [HttpGet]
        public ActionResult AddVisitor()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddVisitor(VISITOR vis)
        {
            string userID = Session["CurrentUserID"].ToString();
            try
            {
                if (ModelState.IsValid)
                {
                    vis.RESIDENT_IC = userID;
                    db.VISITORs.Add(vis);
                    db.SaveChanges();
                    TempData["Success"] = true;
                    return RedirectToAction("ViewVisitor");
                }
                
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
                TempData["ErrorMessage"] = ex;
            }
            return RedirectToAction("ViewVisitor");

        }

        public ActionResult FileReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult VerifyReport(REPORT reportdetail)
        {            
            try
            {
                string userID = Session["CurrentUserID"].ToString();
                if (ModelState.IsValid)
                {                   
                    db.PROC_InsertReport(reportdetail.REPORT_DESC, reportdetail.REPORT_TYPE, userID);
                    TempData["Success"] = true;
                }
            }
            catch (Exception ex)
            {
                TempData["Success"] = false;
                TempData["ErrorMessage"] = ex;
            }

            return RedirectToAction("FileReport","Resident");
        }

        public ActionResult ViewFacility()
        {
            var list_facility = db.FACILITies;
            return View(list_facility);
        }
        

    }
}