using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace XP_Scrum_Grupp2.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult ShowCalendar()
        {
            return View();
        }
    }
}