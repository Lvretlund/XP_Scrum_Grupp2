﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.IO;
using XP_Scrum_Grupp2.Models;

namespace XP_Scrum_Grupp2.Controllers
{
    public class ResearchController : BaseController
    {
       
        // GET: Education
        [Authorize]
        public ActionResult Index(string id)
        {

            var posts = db.ResearchBlogs.Include(x => x.Author).ToList();
            var postIndex = new ResearchIndexViewModel
            {
                ResearchBlog = posts,

            };
            return View(postIndex);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ResearchPartial(ResearchIndexViewModel model, HttpPostedFileBase upload)
        {
            ResearchBlog newPost = new ResearchBlog();
            var userName = User.Identity.Name;

            var author = db.Users.SingleOrDefault(x => x.UserName == userName);


            if (upload != null && upload.ContentLength > 0)
            {
                model.NewResearchBlog.Filename = upload.FileName;
                model.NewResearchBlog.ContentType = upload.ContentType;

                using (var reader = new BinaryReader(upload.InputStream))
                {
                    model.NewResearchBlog.File = reader.ReadBytes(upload.ContentLength);
                }
            }

            newPost.Author = author;
            newPost.Text = model.NewResearchBlog.Text;
            newPost.Date = DateTime.Now;
            newPost.ContentType = model.NewResearchBlog.ContentType;
            newPost.Filename = model.NewResearchBlog.Filename;
            newPost.File = model.NewResearchBlog.File;


            db.ResearchBlogs.Add(newPost);
            db.SaveChanges();
            return RedirectToAction("Index", "Research");
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

        [Authorize]
        [HttpGet]
        public ActionResult Download(int id)
        {
            var fileItem = db.ResearchBlogs.Find(id);
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

    public class ResearchIndexViewModel
    {
        public string Id { get; set; }
        public ICollection<ResearchBlog> ResearchBlog { get; set; }
        public ResearchBlog NewResearchBlog { get; set; } = new ResearchBlog();

    }
}