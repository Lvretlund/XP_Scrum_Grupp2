using XP_Scrum_Grupp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;



namespace XP_Scrum_Grupp2.Controllers
{
    public class BlogController : BaseController
    {
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

        // GET: Blog
        public ActionResult ShowBlogs()
        {
            var cat = db.Categories.ToList();
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                SelectedCategories = PopulateCategories()
            };
            return View(postIndex);
        }

        public ActionResult ShowInformalBlogs()
        {
            var posts = db.InformalBlogs.Include(x => x.Author).ToList();
            var postIndex = new PictureIndexViewModel
            {
                InformalBlogs = posts
            };
            return View(postIndex);
        }

        //post
        [HttpPost]
        public ActionResult CreatePartial(PostIndexViewModel model, HttpPostedFileBase upload)
        {
            var userName = User.Identity.Name;
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
                File = model.NewFormalBlog.File
            };

            if (model.NewCategory.Type == null)
            {
                IEnumerable<SelectListItem> selectedItems = model.SelectedCategories.Where(c => model.CategoryIds.Contains(int.Parse(c.Value))).ToList();
                foreach (var selectedItem in selectedItems)
                {
                    selectedItem.Selected = true;
                    newPost.CategoryN = db.Categories.Where(c => c.Type == selectedItem.Text).FirstOrDefault();
                }
            }
            else
            {

                newPost.CategoryN = model.NewCategory;
            }
            //else
            //{
            //    var existingCat = db.Categories.Where(cat => cat.Type == model.NewCategory.Type).FirstOrDefault();
            //    newPost.CategoryN = existingCat;
            //}
            db.FormalBlogs.Add(newPost);
            db.SaveChanges();
            return RedirectToAction("ShowBlogs", "Blog");
        }

        [HttpPost]
        public ActionResult CreateInformalPartial(PictureIndexViewModel model, HttpPostedFileBase upload)
        {
            InformalBlog newPost = new InformalBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);

            if (upload != null && upload.ContentLength > 0)
            {
                model.NewInformalBlog.Filename = upload.FileName;
                model.NewInformalBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewInformalBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }

            newPost.Author = author;
            newPost.Text = model.NewInformalBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewInformalBlog.ContentType;
            newPost.Filename = model.NewInformalBlog.Filename;
            newPost.File = model.NewInformalBlog.File;


            db.InformalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowInformalBlogs", "Blog");
        }

        public ActionResult Image(int id)
        {
            var post = db.InformalBlogs.Single(x => x.Id == id);
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
    }
    
    public class PostIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<FormalBlog> FormalBlogs { get; set; }
        public FormalBlog NewFormalBlog { get; set; } = new FormalBlog();
        public Category NewCategory { get; set; } = new Category();
        public ICollection<Category> Categories { get; set; }
        public List<SelectListItem> SelectedCategories { get; set; }
        public int[] CategoryIds { get; set; }
        public Category CategoryN { get; set; }
        public ICollection<Meeting> Meetings { get; set; } //testrad
    }

    public class PictureIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<InformalBlog> InformalBlogs { get; set; }
        public InformalBlog NewInformalBlog { get; set; } = new InformalBlog();
    }
}
