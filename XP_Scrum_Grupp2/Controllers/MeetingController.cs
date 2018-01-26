﻿using Microsoft.AspNet.Identity;
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
            var model = new MeetingPeopleViewModel();
            return View(model);
        }

        public ActionResult CreateAMeeting(Meeting meeting)
        {
            var userName = User.Identity.Name;

            var user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            meeting.Creator = user;
            db.Meetings.Add(meeting);
            db.SaveChanges();

            ModelState.Clear();
            return RedirectToAction("ShowCalendar", "Calendar");
        }

        [HttpPost]
        public ActionResult SearchPeople(String txt)
        {
            var model = new MeetingPeopleViewModel();
            model.ApplicationUsers = db.Users.Where(u => (u.Firstname.Contains(txt) || u.Lastname.Contains(txt))).ToList();

            return View("CreateMeeting", model);
        }
    }
}