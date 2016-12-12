using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Infinity.so.Models;
using Microsoft.AspNetCore.Identity;

namespace Infinity.so.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ban> Bans { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Ban>(b =>
            {
                b.Property(p => p.BanDate).ValueGeneratedOnAdd();
            });

        }

        public async void EnsureSeedData(UserManager<ApplicationUser> userMgr, RoleManager<IdentityRole> roleMgr)
        {
            
                // Add 'admin' role

                var archRole = new IdentityRole("Project Lead");
                var engRole = new IdentityRole("Chief Engineer");
                var hadmRole = new IdentityRole("Head of Admins");
                var wikihRole = new IdentityRole("Head of Wiki");
                var forumRole = new IdentityRole("Head of Forum");

                if (await roleMgr.RoleExistsAsync("Project Lead") == false)
                {
                    await roleMgr.CreateAsync(archRole);
                }
                if (await roleMgr.RoleExistsAsync("Chief Engineer") == false)
                {
                    await roleMgr.CreateAsync(engRole);
                }
                if (await roleMgr.RoleExistsAsync("Head of Admins") == false)
                {
                    await roleMgr.CreateAsync(hadmRole);
                }
                if (await roleMgr.RoleExistsAsync("Head of Wiki") == false)
                {
                    await roleMgr.CreateAsync(wikihRole);
                }
                if (await roleMgr.RoleExistsAsync("Head of Forum") == false)
                {
                    await roleMgr.CreateAsync(forumRole);
                }



            if (userMgr.FindByEmailAsync("admin@mydomain.com") == null)
            {
                // create admin user
                var adminUser = new ApplicationUser()
                {
                    UserName = "admin@mydomain.com",
                    Email = "admin@mydomain.com",
                    EmailConfirmed = true,
                    Ckey = "Destinat1on"
                };

                await userMgr.CreateAsync(adminUser, "Ch@ngem35646");

                await userMgr.SetLockoutEnabledAsync(adminUser, false);
                await userMgr.AddToRoleAsync(adminUser, "Chief Engineer");
            }


        }
    }
}
