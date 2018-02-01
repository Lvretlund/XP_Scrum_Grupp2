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
                Firstname = "Kai",
                Lastname = "Wistrand",
                UserName = "kai.wistrand@oru.se",
                Email = "kai.wistrand@oru.se",
                PasswordHash = "AIjXrWn10uFH4NWoTL4oQYeI96rGYL8q409dV2llmCUuAlRpwXQBfPoVKcf9ykMs7w==",
                SecurityStamp = "4d6b574f-f572-43c0-a53b-6e821dde3476",
                Admin = true,
            });

            context.Users.Add(new ApplicationUser
            {
                Firstname = "Ann-Sofie",
                Lastname = "Hellberg",
                UserName = "annsofie.hellberg@oru.se",
                Email = "annsofie.hellberg@oru.se",
                PasswordHash = "AKa0r4QBdjeUX2vzdgx6M5cYQ6OTjZevqwWBloFQ4NG5oLM25AOv2as1ayqqJ/tGUg==",
                SecurityStamp = "1f822c96-a9c5-4940-9f16-b32413a9aed0",
                EducationAdmin = true,
            });
            context.SaveChanges();

            context.Users.Add(new ApplicationUser
            {
                Firstname = "Mathias",
                Lastname = "Hataaka",
                UserName = "mathias.hataaka@oru.se",
                Email = "mathias.hataaka@oru.se",
                PasswordHash = "AERuXg8ASZwSJk3McPYBdI+ZssffmQ/5sN74iCbozO+J/d4TDZIc+bX77wcuPlqTpA==",
                SecurityStamp = "0ec056a8-6be6-4a59-bcbc-12e2c51e7516",
                ResearchAdmin = true,
            });
            context.SaveChanges();
        }
    }
}