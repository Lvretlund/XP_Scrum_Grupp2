﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XP_Scrum_Grupp2.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "This field is required!")]
        public string Type { get; set; }
        public bool IsSelected { get; set; }
        public ICollection<FormalBlog> FormalBlog { get; set; }

  }
}