﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class CalendarController : Controller
    {
        // GET: Calendar
        public ActionResult ShowCalendar()
        {
            return View();
        }

        //public ActionResult GetEvents(double Start, double End)
        //{
        //    var fromDate = ConvertFromUnixTimestamp(Start);
        //    var toDate = ConvertFromUnixTimestamp(End);

        //    //Get the events
        //    //You may get from the repository also
        //    var eventList = GetEvents();

        //    var rows = eventList.ToArray();
        //    return Json(rows, JsonRequestBehavior.AllowGet);
        //}

        //private List<Events> GetEvents()
        //{
        //    List<Events> eventList = new List<Events>();

        //    Events newEvent = new Events
        //    {
        //        id = "1",
        //        title = "Event 1",
        //        start = DateTime.Now.AddDays(1).ToString("s"),
        //        end = DateTime.Now.AddDays(1).ToString("s"),
        //        allDay = false
        //    };


        //    eventList.Add(newEvent);

        //    newEvent = new Events
        //    {
        //        id = "1",
        //        title = "Event 3",
        //        start = DateTime.Now.AddDays(2).ToString("s"),
        //        end = DateTime.Now.AddDays(3).ToString("s"),
        //        allDay = false
        //    };

        //    eventList.Add(newEvent);

        //    return eventList;
        //}

        //private static DateTime ConvertFromUnixTimestamp(double timestamp)
        //{
        //    var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        //    return origin.AddSeconds(timestamp);
        //}
    }
}