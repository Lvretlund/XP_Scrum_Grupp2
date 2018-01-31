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
        // GET: Blog
        public ActionResult ShowBlogs()
        {
            var cat = db.Categories.ToList();
            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
            // var dates = db.FormalBlogs.Include(x => x.Date).ToList();
            var postIndex = new PostIndexViewModel
            {
                FormalBlogs = posts,
                Categories = cat
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

        [HttpPost]
        public ActionResult CreatePartial(PostIndexViewModel model, HttpPostedFileBase upload)
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


            db.FormalBlogs.Add(newPost);
            db.SaveChanges();

            return RedirectToAction("ShowBlogs", "Blog");
        }

        //public ActionResult CreateInformalPartial()
        //{
        //    return View();
        //}

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
            if(post?.File == null)
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
            public Category Category { get; set; } = new Category();
            public ICollection<Category> Categories { get; set; }
            //public string Text { get; set; }
            //public byte[] File { get; set; }
            //public DateTime Date { get; set; }
            //public string Author_Id { get; set; }
            //public string ContentType { get; internal set; }
            //public string Filename { get; internal set; }

        }

        public class PictureIndexViewModel
        {
            public string Id { get; set; }
            public ICollection<InformalBlog> InformalBlogs { get; set; }
            public InformalBlog NewInformalBlog { get; set; } = new InformalBlog();
        }
    }
