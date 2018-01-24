using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<FormalBlog> FormalBlog { get; set; }
        
    }
}