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

        public ActionResult GetMeetings(double start, double end)
        {
            using (var db = new ApplicationDbContext())
            {
                //foreach(var item in db.Meetings)
                //{
                var fromDate = ConvertFromUnixTimestamp(start);
                var toDate = ConvertFromUnixTimestamp(end);
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

        //private List<Meeting> GetEvents()
        //{
        //    List<Meeting> eventList = new List<Meeting>();

        //    var meetings = db.Meetings.ToList();

        //    foreach(var item in meetings)
        //    {
        //        Meeting newEvent = new Meeting
        //        {
        //            Id = item.Id,
        //            Title = item.Title,
        //            Start = item.Start,
        //            //End = item.End,
        //            AllDay = item.AllDay
        //        };
        //        eventList.Add(newEvent);
        //    }

        private List<Events> GetEvents()
        {
            List<Events> eventList = new List<Events>();

            foreach(var item in db.Meetings)
            {
                Events newEvent = new Events
                {
                    id = item.Id.ToString(),
                    title = item.Title,
                    start = item.Start.ToString("s"),
                    end = item.End.ToString("s"),
                    allDay = false
                };
                eventList.Add(newEvent);
            }

            return eventList;
        }



        //    return eventList;
        //}
    }

    public class Events
    {
        public string id { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public string url { get; set; }

        public bool allDay { get; set; }
    }
}