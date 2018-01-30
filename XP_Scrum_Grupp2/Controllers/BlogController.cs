using XP_Scrum_Grupp2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using System.Text;

namespace XP_Scrum_Grupp2.Controllers
{
    public class BlogController : BaseController
    {
        [HttpPost]
        public string Index(IEnumerable<string> selectedCats)
        {
            if (selectedCats == null)
            {
                return "No cities are selected";
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("You selected – " + string.Join(",", selectedCats));
                return sb.ToString();
            }
        }
        
        public ActionResult ShowBlogs()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            if (db.Categories == null)
            {
                Category catis = new Category()
                {
                    Id = 1,
                    Type = "ÅÅÅÅÅ",
                    IsSelected = true
                };
                db.Categories.Add(catis);
                db.SaveChanges();
            }
            List<SelectListItem> listSelectListItems = new List<SelectListItem>();
            foreach (Category cate in db.Categories)
            {
                SelectListItem selectList = new SelectListItem()
                {
                    Text = cate.Type,
                    Value = cate.Id.ToString(),
                    Selected = cate.IsSelected
                };
                listSelectListItems.Add(selectList);
            }
            var cat = db.Categories.ToList();
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            // var dates = db.FormalBlogs.Include(x => x.Date).ToList();
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                Categories = cat,
                Cats = listSelectListItems
            };
            return View(postIndex);
        }

        [HttpPost]
        public ActionResult Create(PostIndexViewModel model, HttpPostedFileBase upload)
        {
            FormalBlog newPost = new FormalBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);


                if (upload != null && upload.ContentLength > 0)
                {
                    model.NewFormalBlog.Filename = upload.FileName;
                    model.NewFormalBlog.ContentType = upload.ContentType;

                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        model.NewFormalBlog.File = reader.ReadBytes(upload.ContentLength);
                    }
                }

            newPost.Author = author;
            newPost.Text = model.NewFormalBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewFormalBlog.ContentType;
            newPost.Filename = model.NewFormalBlog.Filename;
            newPost.File = model.NewFormalBlog.File;
            
            newPost.CategoryN = model.CategoryN;
            Category cat = new Category
            {
                Type = model.Category.Type,
                IsSelected = true
            };
            
            db.FormalBlogs.Add(newPost);
            db.Categories.Add(cat);
            db.SaveChanges();

            return RedirectToAction("ShowBlogs", "Blog" );
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
        public Category Category { get; set; } = new Category();
        public ICollection<Category> Categories { get; set; }
        public IEnumerable<string> SelectedCategories { get; set; }
        public IEnumerable<SelectListItem> Cats { get; set; }
        public Category CategoryN { get; set; }
    }

}