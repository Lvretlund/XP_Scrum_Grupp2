﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class UEvent
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ApplicationUser Creator { get; set; }
        public string CreatorId { get; set; }
        //public double Minutes { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AllDay { get; set; }

        public ApplicationUser User { get; set; }
    }
}