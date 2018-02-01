using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class MeetingController : BaseController
    {
        public static Meeting möte;
        // GET: Meeting
        public ActionResult CreateMeeting()
        {
            return View();
        }

        public ActionResult CreateAMeeting(Meeting meeting)
        {
            var userName = User.Identity.Name;
            var user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            var model = new MeetingPeopleViewModel();

            meeting.Creator = user;
            meeting.Invited = new List<ApplicationUser>();
            meeting.Invited.Add(user);
            db.Meetings.Add(meeting);
            db.SaveChanges();

            möte = meeting;

            model.ApplicationUsers = db.Users.ToList();
            model.Meeting = meeting;

            return View("AddToMeeting", model);
        }
        public ActionResult AddPeople(ApplicationUser person)
        {
            MeetingPeopleViewModel model = new MeetingPeopleViewModel();

            if(!möte.Invited.Contains(person))
            {
                möte.Invited.Add(person);
                db.SaveChanges();
            }

            model.ApplicationUsers = db.Users.ToList();
            model.Meeting = möte;
            return View("AddToMeeting", model);
        }
    }
}

