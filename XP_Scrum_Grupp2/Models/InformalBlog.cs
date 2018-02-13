using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class InformalBlog
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public string Text { get; set; }
        public ApplicationUser Author { get; set; }
        public string Filename { get; set; }
        public string ContentType { get; set; }
        public byte[] File { get; set; }
        public bool Visible { get; set; }
        public string Location { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }

        public virtual ApplicationUser User { get; set; }
       
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


    }
}