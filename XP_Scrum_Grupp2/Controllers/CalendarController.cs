using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class CalendarController : BaseController
    {
        // GET: Calendar
        public ActionResult ShowCalendar()
        {
            return View();
        }

        public ActionResult GetMeetings()
        {
            using (var db = new ApplicationDbContext())
            {
                //foreach(var item in db.Meetings)
                //{
                //    var fromDate = ConvertFromUnixTimestamp(item.Start.ToOADate());
                //    var toDate = ConvertFromUnixTimestamp(item.End.ToOADate());
                //}

                var eventList = GetEvents();

                var rows = eventList.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }
        private static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        private List<Meeting> GetEvents()
        {
            List<Meeting> eventList = new List<Meeting>();

            var meetings = db.Meetings.ToList();

            foreach(var item in meetings)
            {
                Meeting newEvent = new Meeting
                {
                    Id = item.Id,
                    Title = item.Title,
                    Start = item.Start,
                    //End = item.End,
                    AllDay = item.AllDay
                };
                eventList.Add(newEvent);
            }
            
            return eventList;
        }


    }
}