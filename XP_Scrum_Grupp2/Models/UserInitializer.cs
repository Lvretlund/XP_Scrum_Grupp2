using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace XP_Scrum_Grupp2.Models
{
    public class UserInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            var store = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(store);
            base.Seed(context);
            SeedUsers(context);
        }

        private static void SeedUsers(ApplicationDbContext context)
        {

            context.Users.Add(new ApplicationUser
            {
                Id = "1",
                UserName = "kai@hotmail.com",
                Email = "kai@hotmail.com",
                PasswordHash = "Ettbralosenord1!",
                Admin = true,
            });
            context.SaveChanges();
        }
    }
}