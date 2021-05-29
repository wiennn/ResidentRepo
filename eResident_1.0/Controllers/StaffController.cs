using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using eResident_1._0.Models;
using System.Data;
using System.Data.Entity;

namespace eResident_1._0.Controllers
{
    public class StaffController : Controller
    {
        string userID;
        string username;
        eResidentEntities db = new eResidentEntities();
        // GET: Staff
        public ActionResult Index()
        {
            userID = Session["CurrentUserID"].ToString();
            username = Session["CurrentUsername"].ToString();
            return View();
        }
    }
}
