using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class MeetingPeopleViewModel
    {
        public Meeting Meeting { get; set; }
      
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public ApplicationUser User { get; set; }
    }
}