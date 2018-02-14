using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class MeetingController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        public static int MeetingId { get; set; }
        public static List<DateTime> TempStart = new List<DateTime>();
        public static List<DateTime> TempEnd = new List<DateTime>();


        public ActionResult MeetingRequests()
        {
            var userId = User.Identity.GetUserId();
            var currUser = db.Users.Where(u => u.Id == userId).FirstOrDefault();
            var meetingReqs = db.MeetingInvitees.Where(m => m.UserId == userId).ToList();
            var listOfMeet = new List<MeetingInvited>();
            var i = 1;
            foreach (var meet in meetingReqs)
            {
                var meetInv = new MeetingInvited
                {
                    MeetingId = meet.MeetingId,
                    Name = meet.Name,
                    UserId = meet.UserId,
                    Id = i,
                    Meeting = db.Meetings.Where(m => m.Id == meet.MeetingId).FirstOrDefault(),
                    User = db.Users.Where(u => u.Id == meet.UserId).FirstOrDefault()

                };
                i++;
                meetInv.Creator = db.Users.Where(u => u.Id == meetInv.Meeting.CreatorId).FirstOrDefault();
                listOfMeet.Add(meetInv);
            }
            var model = new MeetingInvitedModelTest
            {
                Metinv = listOfMeet
            };
            return View(model);
        }

        public ActionResult SetTime(int id)
        {
            var meetTime = db.MeetingTimes.Where(mt => mt.Id == id).FirstOrDefault();
            var meeting = db.Meetings.Where(m => m.Id == meetTime.MeetingId).FirstOrDefault();

            meeting.Start = meetTime.Time;
            meeting.End = meetTime.Time.AddMinutes(meeting.Minutes);
            meeting.Approved = true;
            db.SaveChanges();

            return RedirectToAction("ShowCalendar", "Calendar");
        }

        public ActionResult RequestedTimes(int meetreq)
        {
            var userId = User.Identity.GetUserId();
            var meetTime = db.MeetingTimes.Where(mt => mt.MeetingId == meetreq).ToList();
            var meeting = db.Meetings.Where(m => m.Id == meetreq).FirstOrDefault();
            var meetingId = meeting.Id;
            var listOfTimes = new List<MeetingTimes>();
            var i = 1;
            foreach (var mt in meetTime)
            {
                var aMeetingTime = new MeetingTimes
                {
                    MeetingId = meetingId,
                    Meeting = meeting,
                    Time = mt.Time,
                    FakeId = mt.Id,
                    Id = i
                };
                listOfTimes.Add(aMeetingTime);
                i++;
            }

            var chosen = db.MetTimInvs.Where(m => m.InvitedId == userId).ToList();
            var model = new MeetTimeInvModel
            {

                ListOfTimes = listOfTimes,
                ListOfChosenTimes = chosen
            };

            return PartialView("_RequestedTimes", model);
        }

        public ActionResult SentTimes(int meetreq)
        {
            var userId = User.Identity.GetUserId();
            var meetTime = db.MeetingTimes.Where(mt => mt.MeetingId == meetreq).ToList();
            var meeting = db.Meetings.Where(m => m.Id == meetreq).FirstOrDefault();
            var meetingId = meeting.Id;
            var listOfTimes = new List<MeetingTimes>();
            var chosen = new List<MeetingTimeInvited>();
            var listofChosen = new List<List<MeetingTimeInvited>>();
            var i = 1;
            foreach (var mt in meetTime)
            {
                chosen = db.MetTimInvs.Where(m => m.TimesId == mt.Id).ToList();
                var aMeetingTime = new MeetingTimes
                {
                    MeetingId = meetingId,
                    Meeting = meeting,
                    Time = mt.Time,
                    FakeId = mt.Id,
                    Id = i,
                    ListofVotes = chosen
                };
                listOfTimes.Add(aMeetingTime);
                i++;
                listofChosen.Add(chosen);
            }
            var model = new MeetTimeInvModelForSent
            {
                ListOfTimes = listOfTimes,
                ListOfChosenTimes = listofChosen
            };

            return PartialView("_SentTimes", model);
        }


        [HttpPost]
        public ActionResult ChooseTime(int Id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var user = User.Identity.GetUserId();
            var username = db.Users.Where(u => u.Id == user).FirstOrDefault();
            var name = username.Firstname + " " + username.Lastname;
            MeetingTimes time = db.MeetingTimes.Where(f => f.Id == Id).FirstOrDefault();

            var newMetTimInv = new MeetingTimeInvited
            {
                MeetingId = time.MeetingId,
                InvitedId = user,
                InvitedName = name,
                TimesId = Id
            };
            db.MetTimInvs.Add(newMetTimInv);
            db.SaveChanges();
            return RedirectToAction("Voted", "Meeting");
        }

        public ActionResult Voted()
        {
            return View();
        }

        // GET: Meeting
        public ActionResult CreateMeeting()
        {
            TempStart.Clear();
            var model = new Meeting
            {
                Start = DateTime.Now
            };
            return View(model);
        }

        public ActionResult SaveMeeting(MeetingTimes meet)
        {
            var meeting = db.Meetings.Where(m => m.Id == meet.MeetingId).FirstOrDefault();
            var newMeet = new Meeting
            {
                Start = meet.Time,
                End = meeting.End,
                AllDay = meeting.AllDay,
                CreatorId = meeting.CreatorId,
                Id = meet.MeetingId,
                Minutes = meeting.Minutes,
                Approved = true,
                Title = meeting.Title
            };
            db.Meetings.Remove(meeting);
            db.Meetings.Add(newMeet);
            db.SaveChanges();
            return RedirectToAction("Voted");
        }

        [HttpPost]
        public ActionResult AddTempTime(Meeting m)
        {
            if (m.Start.Year != 1 && m.Start.Year != 1)
            {
                TempStart.Add(m.Start);
            }
            var nm = new Meeting
            {
                Start = m.Start,
                Times = TempStart
            };
            return View("CreateMeeting", nm);
        }

        public void SaveTimes(Meeting meeting)
        {
            foreach (var time in TempStart)
            {
                var mt = new MeetingTimes
                {
                    MeetingId = meeting.Id,
                    Meeting = meeting,
                    Time = time
                };
                db.MeetingTimes.Add(mt);
                db.SaveChanges();
            }
        }
        [HttpPost]
        public ActionResult CreateAMeeting(Meeting m)
        {
            if (ModelState.IsValid)
            {
                var userName = User.Identity.Name;
                var user = db.Users.FirstOrDefault(u => u.UserName == userName);

                Meeting newm = new Meeting
                {
                    Creator = user,
                    CreatorId = user.Id,
                    Title = m.Title,
                    Times = TempStart,
                    Start = m.Start,
                    Minutes = m.Minutes
                };
                newm.End = m.Start.AddMinutes(m.Minutes);
                db.Meetings.Add(newm);
                db.SaveChanges();
                MeetingId = newm.Id;
                SaveTimes(newm);
                TempStart.Clear();
                var users = db.Users.ToList();
                var model = new MeetingInvited
                {
                    Meeting = newm,
                    MeetingId = newm.Id,
                    User = user,
                    UserId = user.Id,
                    Uninvited = db.Users.ToList()
                };


                return View("AddToMeeting", model);
            }
            else
            {
                return RedirectToAction("CreateMeeting");
            }
        }

        public async Task<ActionResult> AddPeople(MeetingInvited us)
        {
            var invited = db.Users.Where(u => u.Id == us.UserId).FirstOrDefault();
            var meeting = db.Meetings.Where(m => m.Id == MeetingId).FirstOrDefault();
            var mi = new MeetingInvited
            {
                User = invited,
                UserId = us.UserId,
                Name = invited.Firstname + " " + invited.Lastname,
                Meeting = meeting,
                MeetingId = meeting.Id
            };
            bool exist = db.MeetingInvitees.Where(m => m.UserId == us.UserId).Where(m => m.MeetingId == us.MeetingId).Any();
            if (!exist)
            {
                db.MeetingInvitees.Add(mi);
                db.SaveChanges();
            }
            var invitees = db.MeetingInvitees.Where(m => m.MeetingId == us.MeetingId).ToList();
            var listaInv = new List<ApplicationUser>();
            foreach (var inv in invitees)
            {
                listaInv.Add(db.Users.Where(u => u.Id == inv.UserId).FirstOrDefault());
            }
            var nm = new MeetingInvited
            {
                Uninvited = new List<ApplicationUser>(),
                Meeting = meeting,
                MeetingId = meeting.Id,
                Invited = listaInv
            };
            var all = db.Users.ToList();
            foreach (var u in all)
            {
                bool exists = db.MeetingInvitees.Where(m => m.UserId == u.Id).Where(m => m.MeetingId == us.MeetingId).Any();
                if (u.Id != us.UserId)
                {
                    if (!exists)
                    {
                        nm.Uninvited.Add(u);
                    }
                }
            }
            bool isfalse = false;
            if (isfalse == true)
            {
                await SignInManager.SignInAsync(us.User, isPersistent: false, rememberBrowser: false);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(us.UserId);
                await UserManager.SendEmailAsync(us.UserId, "You have received a new meeting request", "Please visit the site to see meeting invitation.");
            }
            return View("AddToMeeting", nm);
        }

        public ActionResult SentMeetingRequests()
        {
            var userName = User.Identity.Name;
            var user = db.Users.FirstOrDefault(u => u.UserName == userName);
            var model = db.Meetings.Where(m => m.CreatorId == user.Id).ToList();
            var models = new SentMeeting
            {
                SentMeetings = model
            };
            return View(models);
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
    }

    public class MeetingInvitedModelTest
    {
        public List<MeetingInvited> Metinv { get; set; }
    }

    public class SentMeeting
    {
        public List<Meeting> SentMeetings { get; set; }
    }
}

