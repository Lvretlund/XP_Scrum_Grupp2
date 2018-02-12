using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class MeetingInvited
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public int MeetingId { get; set; }
        public ICollection<ApplicationUser> Uninvited {get;set;}
        public List<ApplicationUser> Invited { get; set; }
        public ApplicationUser Creator { get; set; }

        //--------------
        public ApplicationUser User { get; set; }
        public Meeting Meeting { get; set; }
    }
}

