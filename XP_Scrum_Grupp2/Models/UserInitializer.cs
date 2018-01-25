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
                PasswordHash = "ANN2a+ErNxzW8mljt8Emw33ZW1zGg74vzQnUYLy7XrfF1mz95YJLlZYoRm52/zz8vA==",
                SecurityStamp = "a33b68b8-38f9-4ba1-a8d6-6abbe91d3da6",
                Admin = true,
            });
            context.SaveChanges();
        }
    }
}