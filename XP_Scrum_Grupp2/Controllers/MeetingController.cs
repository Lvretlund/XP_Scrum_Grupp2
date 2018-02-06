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
        public static Meeting NewMeeting;
        public static List<DateTime> TempTimes = new List<DateTime>();
        // GET: Meeting
        public ActionResult CreateMeeting()
        {
            return View();
        }

        //public ActionResult DeleteMeeting(Meeting MeetingModel)
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    db.Meetings.Remove(model);
        //    db.SaveChanges();
        //    return View("Index", "Home");
        //}

        public ActionResult CreateAMeeting(Meeting meeting)
        {
            var userName = User.Identity.Name;
            var user = db.Users.Where(u => u.UserName == userName).SingleOrDefault();
            var model = new MeetingPeopleViewModel();

            meeting.Creator = user;
            meeting.Invited = new List<ApplicationUser>();
            meeting.Invited.Add(user);
            meeting.Times = TempTimes;
            meeting.Start = DateTime.Now;
            db.Meetings.Add(meeting);
            db.SaveChanges();

            NewMeeting = meeting;

            model.ApplicationUsers = new LinkedList<ApplicationUser>();
            var users = db.Users.ToList();
            var fe = false;

            foreach (var item in users)
            {
                foreach (var i in NewMeeting.Invited)
                {
                    if (item.Email.Equals(i.Email))
                    {
                        fe = true;
                    }
                }
                if (fe == false)
                {
                    model.ApplicationUsers.Add(item);
                }
                fe = false;
            }

           // model.ApplicationUsers = db.Users.ToList();
            model.Meeting = meeting;

            return View("AddToMeeting", model);
        }
        public ActionResult AddPeople(ApplicationUser person)
        {
            MeetingPeopleViewModel model = new MeetingPeopleViewModel();

            if(!NewMeeting.Invited.Contains(person))
            {
                NewMeeting.Invited.Add(person);
                db.SaveChanges();
            }

            model.ApplicationUsers = new List<ApplicationUser>();
            var users = db.Users.ToList();
            var fe = false;

            foreach (var item in users)
            {
                foreach (var i in NewMeeting.Invited)
                {
                    if (item.Email.Equals(i.Email))
                    {
                        fe = true;
                    }
                }
                if(fe == false)
                {
                    model.ApplicationUsers.Add(item);
                }
                fe = false;
            }

            model.Meeting = NewMeeting;

            return View("AddToMeeting", model);
        }

        public ActionResult AddTempTime(DateTime Start, Meeting meeting)
        {
            TempTimes.Add(Start);
            meeting.Times = TempTimes;
            return View("CreateMeeting", meeting);
        }
    }
}

