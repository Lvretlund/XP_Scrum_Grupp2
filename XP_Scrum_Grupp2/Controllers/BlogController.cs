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
        public ActionResult ShowBlogs(string id)
        {

            var posts = db.FormalBlogs.Include(x => x.Author).ToList();
           // var dates = db.FormalBlogs.Include(x => x.Date).ToList();

            return View(new PostIndexViewModel { Id = id, FormalBlogs = posts});


        }

        [HttpPost]
        public ActionResult Create(PostIndexViewModel model, HttpPostedFileBase upload)
        {
            FormalBlog newPost = new FormalBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);


                if (upload != null && upload.ContentLength > 0)
                {
                    model.Filename = upload.FileName;
                    model.ContentType = upload.ContentType;

                    using (var reader = new BinaryReader(upload.InputStream))
                    {
                        model.File = reader.ReadBytes(upload.ContentLength);
                    }
                }

            newPost.Author = author;
            newPost.Text = model.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.ContentType;
            newPost.Filename = model.Filename;
            newPost.File = model.File;
            

            db.FormalBlogs.Add(newPost);
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
        public string Text { get; set; }
        public byte[] File { get; set; }
        public DateTime Date { get; set; }
        public string Author_Id { get; set; }
        public string ContentType { get; internal set; }
        public string Filename { get; internal set; }
        
    }

}