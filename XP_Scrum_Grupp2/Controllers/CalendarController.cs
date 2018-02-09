using Microsoft.AspNet.Identity;
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
        public JsonResult GetUEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var userId = User.Identity.GetUserId();
                var events = db.UserEvents.Where(u => u.Creator.Id == userId).ToList();
                var me = db.Meetings.Where(m => m.Creator.Id == userId).ToList();
                var model = new PersonalCalendarViewModel
                {
                    PersonalEvents = events,
                    Meetings = me
                };
                return new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(UEvent e)
        {
            var user = User.Identity.GetUserId();
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var curr = db.Users.Where(u => u.Id == user).FirstOrDefault();
                var v = new UEvent
                {
                    Title = e.Title,
                    Start = e.Start,
                    End = e.End,
                    AllDay = e.AllDay,
                    Creator = curr
                };
                    db.UserEvents.Add(v);
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var v = db.UserEvents.Where(a => a.Id == eventID).FirstOrDefault();
                if (v != null)
                {
                    db.UserEvents.Remove(v);
                    db.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult ShowPersonalCalendar()
        {
            var userId = User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();
            var ue = db.UserEvents.Where(u => u.Creator.Id == userId).ToList();
            var me = db.Meetings.Where(m=> m.Creator.Id == userId).ToList();
            var model = new PersonalCalendarViewModel {
            PersonalEvents = ue,
            Meetings = me
            };
            return View(model);
        }
        public ActionResult ShowCalendar()
        {
            return View();
        }

        public ActionResult GetMeetings(double start, double end)
        {
            using (var db = new ApplicationDbContext())
            {
                var fromDate = ConvertFromUnixTimestamp(start);
                var toDate = ConvertFromUnixTimestamp(end);
                
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

        private List<Events> GetEvents()
        {
            List<Events> eventList = new List<Events>();

            foreach (var item in db.Meetings)
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

    public class PersonalCalendarViewModel
    {
        public ICollection<UEvent> PersonalEvents { get; set; }
        public ICollection<Meeting> Meetings { get; set; }
    }
}