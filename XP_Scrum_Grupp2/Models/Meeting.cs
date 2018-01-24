using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public ApplicationUser Creator { get; set; }
        public ICollection<DateTime> Time { get; set; }
        public ICollection<ApplicationUser> Invited { get; set; }
        public DateTime ChosenTime { get; set; }

        public ICollection<ApplicationUser> User { get; set; }
    }
}