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
        public static Meeting möte;
        public static List<DateTime> TempTimes = new List<DateTime>();
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
            meeting.Times = TempTimes;
            meeting.Start = DateTime.Now;
            db.Meetings.Add(meeting);
            db.SaveChanges();

            möte = meeting;

            model.ApplicationUsers = new LinkedList<ApplicationUser>();
            var users = db.Users.ToList();
            var fe = false;

            foreach (var item in users)
            {
                foreach (var i in möte.Invited)
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


        public async Task<ActionResult> AddPeople(ApplicationUser person)
        {
            MeetingPeopleViewModel model = new MeetingPeopleViewModel();

            if(!möte.Invited.Contains(person))
            {
                möte.Invited.Add(person);
                db.SaveChanges();
            }

            model.ApplicationUsers = new List<ApplicationUser>();
            var users = db.Users.ToList();
            var fe = false;

            foreach (var item in users)
            {
                foreach (var i in möte.Invited)
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

            model.Meeting = möte;

            
                var userN = new ApplicationUser { Id = person.Id, UserName = person.Email, Email = person.Email};
                //userN.Admin = false;

                await SignInManager.SignInAsync(userN, isPersistent: false, rememberBrowser: false);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(userN.Id);
                await UserManager.SendEmailAsync(userN.Id, "Meeting conformation", "Please visit the site to see meeting invitations");
            

                return View("AddToMeeting", model);
        }

        public ActionResult AddTempTime(DateTime Start, Meeting meeting)
        {
            TempTimes.Add(Start);
            meeting.Times = TempTimes;
            return View("CreateMeeting", meeting);
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



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Notification(MeetingPeopleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.User.Email, Email = model.User.Email, Firstname = model.User.Firstname, Lastname = model.User.Lastname };
                user.Admin = false;

                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                await UserManager.SendEmailAsync(user.Id, "Notis", "Please visit the site to see notifications");
                return RedirectToAction("Index", "Home");

            }
            return View();
        }
    }
    
}

