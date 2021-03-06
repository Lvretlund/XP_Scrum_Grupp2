﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class EducationBlog
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ApplicationUser Author { get; set; }
        public DateTime Date { get; set; }
        public byte[] File { get; set; }
        public string ContentType { get; set; }
        public string Filename { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}