using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class InformalBlog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public byte Image { get; set; }
        public string Text { get; set; }
        public ApplicationUser Author { get; set; }

        public virtual ApplicationUser User { get; set; }

        


    }
}