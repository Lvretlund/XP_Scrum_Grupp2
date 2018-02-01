using System.Collections.Generic;

namespace XP_Scrum_Grupp2.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<FormalBlog> FormalBlog { get; set; }
  
        public string hello { get; set; }

  }
}