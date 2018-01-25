using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections;
using System.Collections.Generic;

namespace XP_Scrum_Grupp2.Models
{   


    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public ICollection<FormalBlog> FormalBlog { get; set; }
        public ICollection<InformalBlog> InformalBlog { get; set; }
        public ICollection<ResearchBlog> ResearchBlog { get; set; }
        public ICollection<EducationBlog> EducationBlog { get; set; }
        public bool Admin { get; set; }
        public string Tjo { get; set; }

        public ICollection<Meeting> Meeting { get; set; }


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
        public DbSet<EducationBlog> EducationBlogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        
        
        
    }
    public class DataContextInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {

    }
}