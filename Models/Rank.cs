using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infinihub.Models
{
    public class Rank : IdentityRole
    {
        public Rank() : base() { }
        public Rank(string Role, AdminPermissions permissions) : base(Role)
        {
            Permissions = permissions;
        }
        public AdminPermissions Permissions { get; set; }
    }


}
