using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public ApplicationUser Username { get; set; }
        public virtual FormalBlog Post { get; set; }
    }
}