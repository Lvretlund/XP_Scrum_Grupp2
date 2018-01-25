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
                Forname = "Kai",
                Lastname = "Wistrand",
                UserName = "kai.wistrand@oru.se",
                Email = "kai.wistrand@oru.se",
                PasswordHash = "AIjXrWn10uFH4NWoTL4oQYeI96rGYL8q409dV2llmCUuAlRpwXQBfPoVKcf9ykMs7w==",
                SecurityStamp = "4d6b574f-f572-43c0-a53b-6e821dde3476",
                Admin = true,
            });
            context.SaveChanges();
        }
    }
}