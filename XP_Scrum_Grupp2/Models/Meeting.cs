using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Meeting
    {
       
        public int Id { get; set; }
        public string Title { get; set; }
        public ApplicationUser Creator { get; set; }
        public ICollection<ApplicationUser> Invited { get; set; }
        public double Minutes { get; set; }
        public DateTime Start { get; set; }
        public ICollection<DateTime> Times { get; set; }
        public bool AllDay { get; set; }

        public ICollection<ApplicationUser> User { get; set; }
        public ApplicationUser UserNotis { get; set; }
        
    }
}