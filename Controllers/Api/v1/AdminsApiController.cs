//   Copyright 2017 Solaris 13 Foundation
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

//       http://www.apache.org/licenses/LICENSE-2.0

//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
using AspNet.Security.OAuth.Validation;
using Infinihub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Controllers.Api.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admins/")]
    [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Host")]
    public class AdminsApiController : Controller
    {
        /*
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AdminsApiController(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpGet]
        public async IActionResult ListStaff()
        {
            await _roleManager.FindByName("Staff").Result.Users.;
        } 


        private async Task<List<ApplicationUser>> GetStaff()
        {
            var role = await _roleManager.FindByNameAsync("Staff");
            return role.Users.Select(e => e.UserId).ToList();
        }
        */
    }
}
