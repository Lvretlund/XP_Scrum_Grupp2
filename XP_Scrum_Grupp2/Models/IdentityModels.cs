using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;
using System;

namespace XP_Scrum_Grupp2.Models
{

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        
        public bool Admin { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public bool EducationAdmin { get; set; }
        public bool ResearchAdmin { get; set; }
        public bool NewFormalPostsNotification { get; set; }


        public ICollection<UEvent> UserEvents { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Meeting> Meeting { get; set; }
        public ICollection<FormalBlog> FormalBlog { get; set; }
        public ICollection<InformalBlog> InformalBlog { get; set; }
        public ICollection<ResearchBlog> ResearchBlog { get; set; }
        public ICollection<EducationBlog> EducationBlog { get; set; }
        


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("dataContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<ResearchBlog> ResearchBlogs { get; set; }
        public DbSet<InformalBlog> InformalBlogs { get; set; }
        public DbSet<FormalBlog> FormalBlogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EducationBlog> EducationBlogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MeetingInvited> MeetingInvited { get; set; }
        public DbSet<MeetingTimes> MeetingTimes { get; set; }
        public DbSet<InformalComment> InformalComments { get; set; }
        public DbSet<UEvent> UserEvents { get; set; }
        public bool NewFormalPostsNotification { get; internal set; }

        public class DataContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
        {

        }
    }

    public class InformalComment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ApplicationUser CommentedBy { get; set; }
        public string CommentedById { get; set; }
        public InformalBlog Post { get; set; }
        public DateTime Date { get; set; }


        public virtual ApplicationUser User { get; set; }
        public virtual InformalBlog InformalBlog { get; set; }
    }
}