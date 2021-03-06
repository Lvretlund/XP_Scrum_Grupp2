﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Meeting
    {
     
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public ApplicationUser Creator { get; set; }
        public string CreatorId { get; set; }
        [Required]
        public double Minutes { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ICollection<DateTime> Times { get; set; }
        public bool AllDay { get; set; }
        public bool Approved { get; set; }

        public ICollection<MeetingInvited> MeetingInvited { get; set; }
        public ApplicationUser UserNotis { get; set; }
        public ICollection<ApplicationUser> User { get; set; }
        
    }
}