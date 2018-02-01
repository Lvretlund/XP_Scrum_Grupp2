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

            model.ApplicationUsers = db.Users.ToList();
            model.Meeting = meeting;

            return View("AddPeople", model);
        }
        public ActionResult AddPeople(string id, Meeting meeting)
        {
            var person = db.Users.Where(u => u.Id == id).SingleOrDefault();
            MeetingPeopleViewModel model = new MeetingPeopleViewModel();

            if(!meeting.Invited.Contains(person))
            {
                meeting.Invited.Add(person);
                db.SaveChanges();
            }

            model.ApplicationUsers = db.Users.ToList();
            model.Meeting = meeting;
            return View("AddPeople", model);
        }
    }
}

