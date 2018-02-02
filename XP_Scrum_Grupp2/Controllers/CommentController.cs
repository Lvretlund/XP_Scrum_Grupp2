using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class CommentController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

       

        [HttpPost]
        public ActionResult AddComment(Comment comment, int postId, string Id)
        {
            string userId = comment.CommentedById;
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            var post = db.FormalBlogs.FirstOrDefault(p => p.Id == postId);
            if (comment != null)
            {
                var NewComment = new Comment
                {
                    Id = 1,
                    Text = comment.Text,
                    Date = comment.Date,
                    CommentedBy = user,
                    CommentedById = userId,
                    User = user,
                    Post = post,
                    FormalBlog = post
                };
                if (user != null && post != null)
                {
                    db.Comments.Add(NewComment);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("GetComments", "Comment", new { postId = postId });
        }

        public PartialViewResult GetComments(int postId)
        {

            CommentModel cm = new CommentModel
            {
                Commentarer = db.Comments.Where(c => c.Post.Id == postId).ToList()
                //.Select(c => new Comment
                //{
                //    Id = c.Id,
                //    Date = c.Date,
                //    Text = c.Text,
                //    CommentedBy = new ApplicationUser
                //    {
                //        Id = c.CommentedBy.Id,
                //        Firstname = c.CommentedBy.Firstname,
                //        Lastname = c.CommentedBy.Lastname
                //    }
                //}).ToList()
            };
            return PartialView("~/Views/Shared/_Comments.cshtml", cm);
        }
    }
}