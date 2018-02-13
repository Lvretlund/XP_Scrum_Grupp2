using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class Comment
    {

        public int Id { get; set; }
        
        [Required]
        public string Text { get; set; }
        public ApplicationUser CommentedBy { get; set; }
        public string CommentedById { get; set; }
        public FormalBlog Post { get; set; }
        public DateTime Date { get; set; }
        

        public virtual ApplicationUser User { get; set; }
        public virtual FormalBlog FormalBlog { get; set; }
    }

    public class CommentModel
    {
        public ICollection<Comment> Commentarer { get; set; }
        public ICollection<InformalComment> IComments { get; set; }
    }
}