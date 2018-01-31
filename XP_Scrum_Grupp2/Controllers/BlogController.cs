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

        //get
        public ActionResult ShowBlogs()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var cat = db.Categories.ToList();
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                SelectedCategories = PopulateCategories()
            };
            return View(postIndex);
        }

        //post
        [HttpPost]
        public ActionResult Create(PostIndexViewModel model, HttpPostedFileBase upload)
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
        public List<SelectListItem> SelectedCategories { get; set; }
        public int[] CategoryIds { get; set; }
        public Category CategoryN { get; set; }
    }

}