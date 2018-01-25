﻿using System;
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
        public ICollection<DateTime> Dates { get; set; }
        public ICollection<ApplicationUser> Invited { get; set; }
        public DateTime ChosenDate { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public bool AllDay { get; set; }

        public ICollection<ApplicationUser> User { get; set; }
    }
}