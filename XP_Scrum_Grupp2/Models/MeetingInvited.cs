using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class MeetingInvited
    {
        [Key]
        public int Id { get; set; }
        public Meeting MeetingId { get; set; } 
        public ApplicationUser Invited { get; set; }
    }
}