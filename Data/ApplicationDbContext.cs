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

                var archRole = new IdentityRole("Host");
                var engRole = new IdentityRole("Game Admin");
                var hadmRole = new IdentityRole("Game Master");
                var wikihRole = new IdentityRole("Trial Admin");
                var forumRole = new IdentityRole("Developer");

                if (await roleMgr.RoleExistsAsync("Host") == false)
                {
                    await roleMgr.CreateAsync(archRole);
                }
                if (await roleMgr.RoleExistsAsync("Game Admin") == false)
                {
                    await roleMgr.CreateAsync(engRole);
                }
                if (await roleMgr.RoleExistsAsync("Game Master") == false)
                {
                    await roleMgr.CreateAsync(hadmRole);
                }
                if (await roleMgr.RoleExistsAsync("Trial Admin") == false)
                {
                    await roleMgr.CreateAsync(wikihRole);
                }
                if (await roleMgr.RoleExistsAsync("Developer") == false)
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
