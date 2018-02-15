using XP_Scrum_Grupp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using Microsoft.AspNet.Identity.Owin;

namespace XP_Scrum_Grupp2.Controllers
{
    public class BlogController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        [HttpPost]
        public ActionResult HidePost(int postId, bool status)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            FormalBlog fb = db.FormalBlogs.Where(f => f.Id == postId).FirstOrDefault();
            if(status == true) {
                fb.Visible = false;
            } else {
                fb.Visible = true; }
            db.SaveChanges();
            return RedirectToAction("ShowBlogs", "Blog");
        }

        private static List<SelectListItem> PopulateCategories()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (Category cate in db.Categories)
            {
                items.Add(new SelectListItem
                {
                    Text = cate.Type.ToString(),
                    Value = cate.Id.ToString(),
                    Selected = cate.IsSelected
                });
            }
            return items;
        }

        [Authorize]
        public ActionResult ShowBlogs()
        {
            var cat = db.Categories.ToList();
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            var comments = db.Comments.Include(f => f.FormalBlog).ToList();
            
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                SelectedCategories = PopulateCategories(),
                Comments = comments
            };
            return View(postIndex);
        }
        
        [HttpPost]
        public ActionResult CreatePartial(PostIndexViewModel model, HttpPostedFileBase upload)
        {
            var userName = User.Identity.Name;
            var post = db.FormalBlogs.ToList();
            var author = db.Users.SingleOrDefault(x => x.UserName == userName);
            model.SelectedCategories = PopulateCategories();
            if (upload != null && upload.ContentLength > 0)
            {
                model.NewFormalBlog.Filename = upload.FileName;
                model.NewFormalBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewFormalBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }
            FormalBlog newPost = new FormalBlog
            {
                Author = author,
                Text = model.NewFormalBlog.Text,
                Date = DateTime.Now,
                ContentType = model.NewFormalBlog.ContentType,
                Filename = model.NewFormalBlog.Filename,
                File = model.NewFormalBlog.File,
                Visible = true
            };

            var exists = db.Categories.Where(c => c.Type == model.NewCategory.Type).FirstOrDefault();
            if (exists != null)
            {
                newPost.CategoryN = exists;
            }
            else
            {
                newPost.CategoryN = model.NewCategory; 
            }

            db.FormalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowBlogs", "Blog");
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Image(int id)
        {
            var post = db.FormalBlogs.Single(x => x.Id == id);
            if (post?.File == null)
            {
                return HttpNotFound();
            }
            return File(post.File, post.ContentType);
        }

        [HttpGet]
        public ActionResult Download(int id)
        {
            var fileItem = db.FormalBlogs.Find(id);
            if (fileItem?.File == null)
            {
                return HttpNotFound();
            }
            var response = new FileContentResult(fileItem.File, fileItem.ContentType)
            {
                FileDownloadName = fileItem.Filename
            };

            return response;
        }

        public ActionResult SearchFiles()
        {
            return View();
        }
        public ActionResult Search(int? startYear, int? startMonth, int? endYear, int? endMonth)
        {
            if (startYear != null && endYear != null && startMonth != null && endMonth != null )
            {
                DateTime startDate = new DateTime(startYear.Value, startMonth.Value, 1);
                DateTime endDate = new DateTime(endYear.Value, endMonth.Value + 1, 1);

                var files = db.FormalBlogs.Where(f => f.Date >= startDate && f.Date <= endDate).Where(f => f.Filename != null).ToList();



                return View("SearchFiles", files);

            }
            ViewBag.Message = "Select start and end year/month";
            return View("SearchFiles");

        }
    }
    
    public class PostIndexViewModel
    {
        public ICollection<InformalBlog> InformalBlogs { get; set; }
        public string Id { get; set; }
        public ICollection<FormalBlog> FormalBlogs { get; set; }
        public FormalBlog NewFormalBlog { get; set; } = new FormalBlog();
        public Category NewCategory { get; set; } = new Category();
        public ICollection<Category> Categories { get; set; }
        public List<SelectListItem> SelectedCategories { get; set; }
        public int[] CategoryIds { get; set; }
        public Category CategoryN { get; set; }
        public ICollection<Meeting> Meetings { get; set; } 
        public ICollection<Comment> Comments { get; set; }
        public string Text { get; set; }
        public string Location { get; set; }
        public ApplicationUser UserName { get; set; }
        public ICollection<InformalComment> InformalComments { get; set; }
        public InformalBlog NewInformalBlog { get; set; } = new InformalBlog();
    }
}
