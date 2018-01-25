using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class MeetingController : Controller
    {
        // GET: Meeting
        public ActionResult CreateMeeting()
        {
            return View();
        }

        public ActionResult CreateAMeeting(Meeting meeting)
        {
            var userName = User.Identity.Name;
            using (var db = new ApplicationDbContext()) {
                var user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
                meeting.Creator = user;
                db.Meetings.Add(meeting);
                db.SaveChanges();
            }
            ModelState.Clear();
            return View("CreateMeeting");
        }
    }
}