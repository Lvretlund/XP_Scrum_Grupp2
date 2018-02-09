using System.Linq;
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

        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult AddIComment(InformalComment comment, int postId, string Id)
        {
            string userId = comment.CommentedById;
            var user = db.Users.FirstOrDefault(u => u.Id == userId);
            var post = db.InformalBlogs.FirstOrDefault(p => p.Id == postId);
            if (comment != null)
            {
                var NewComment = new InformalComment
                {
                    Id = 1,
                    Text = comment.Text,
                    Date = comment.Date,
                    CommentedBy = user,
                    CommentedById = userId,
                    User = user,
                    Post = post,
                    InformalBlog = post
                };
                if (user != null && post != null)
                {
                    db.InformalComments.Add(NewComment);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("GetIComments", "Comment", new { postId = postId });
        }

        public PartialViewResult GetComments(int postId)
        {
            CommentModel cm = new CommentModel
            {
                Commentarer = db.Comments.Where(c => c.Post.Id == postId).ToList()
            };
            return PartialView("~/Views/Shared/_Comments.cshtml", cm);
        }

        public PartialViewResult GetIComments(int postId)
        {
            CommentModel cm = new CommentModel
            {
                IComments = db.InformalComments.Where(c => c.Post.Id == postId).ToList()
            };
            return PartialView("~/Views/Shared/_IComments.cshtml", cm);
        }
    }
}