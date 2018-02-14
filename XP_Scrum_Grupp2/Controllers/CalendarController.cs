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
                var events = db.UserEvents.Where(u => u.CreatorId == userId).ToList();
                var me = db.Meetings.Where(m => m.CreatorId == userId).ToList();
                var model = new PersonalCalendarViewModel
                {
                    PersonalEvents = events,
                    Meetings = me
                };
                return new JsonResult { Data = model, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult ShowInvited(int meetingId)
        {
            var model = db.MeetingInvitees.Where(m => m.MeetingId == meetingId).ToList();
            return PartialView("_Invited", model);
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
                    Creator = curr,
                    CreatorId = curr.Id
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
            var me = db.Meetings.Where(m => m.CreatorId == userId).ToList();
            var model = new PersonalCalendarViewModel
            {
                PersonalEvents = ue,
                Meetings = me
            };
            return View(model);
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var me = db.Meetings.Where(m=>m.Approved == true).ToList();
                return new JsonResult { Data = me, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult ShowCalendar()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var meetings = db.Meetings.Where(m => m.Approved == true).ToList();
            var inviteds = new List<List<MeetingInvited>>
            {
                new List<MeetingInvited>()
            };
            foreach (var m in meetings)
            {
                var o = db.MeetingInvitees.Where(me => me.MeetingId == m.Id).ToList();
                inviteds.Add(o);
            }
            var model = new MeetingInvitedModel {
                Meetinginvited = inviteds
            };
            return View(model);
        }
    }
        public class MeetingInvitedModel
    {
        public List<List<MeetingInvited>> Meetinginvited { get; set; }

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