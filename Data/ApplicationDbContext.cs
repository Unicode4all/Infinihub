using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Infinihub.Models;
using Infinity.so.Models;

namespace Infinity.so.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Rank> Ranks { get; set; }
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

                var archRole = new Rank("Host",AdminPermissions.R_EVERYTHING);
                var engRole = new Rank("Game Admin", AdminPermissions.R_BAN|AdminPermissions.R_ADMIN|AdminPermissions.R_DEBUG);
                var hadmRole = new Rank("Game Master", AdminPermissions.R_INVESTIGATE);
                var wikihRole = new Rank("Trial Admin", AdminPermissions.R_DEBUG);
                var forumRole = new Rank("Developer", AdminPermissions.R_DEBUG);

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



            if (await userMgr.FindByEmailAsync("admin@mydomain.com") == null)
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
                await userMgr.AddToRoleAsync(adminUser, "Host");
            }


        }
    }
}
