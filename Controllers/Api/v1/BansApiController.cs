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
using Infinihub.Data;
using Infinihub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Threading.Tasks;

namespace Infinihub.Controllers.Api
{
    [ApiVersion("1.0")]
    [Authorize(Policy = "RequireAdminScope")]
    [Route("api/v{version:apiVersion}/bans/")]
    public class BansApiController : Controller
    {
        private readonly IBanRepository _banRepository;

        public BansApiController(IBanRepository banRepository)
        {
            _banRepository = banRepository;
        }

        [HttpGet, Produces("application/json")]
        [AllowAnonymous]
        public IActionResult GetAll()
        {
            return Json(_banRepository.GetAll());
        } 

        [HttpGet("{id}", Name = "GetBan")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int id)
        {
            Ban ban = await _banRepository.FindByIdAsync(id);
            

            if (ban == null)
                return NotFound();

            return Json(await _banRepository.FindByIdAsync(id));
        }

        [HttpGet("find"), Produces("application/json")]
        public IActionResult FindBan(string ckey, string cid, string ip)
        {
            var ban = _banRepository.Find(ckey, cid, ip);

            if (ban == null)
                return NotFound();

           // ban.Reason = EncodeOutput(ban.Reason);
            return Json(ban);
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Host")]
        [HttpPost("add")]
        public async Task<IActionResult> AddBan([Bind("BanExpiryTime", "SubjectIPAddress", "SubjectCkey", "SubjectCid", "AdminCkey", "BanType", "Job", "Reason")][FromBody] Ban ban)
        {
            if (ban == null)
                return BadRequest();

            await _banRepository.Add(ban);
            return CreatedAtRoute("GetBan", new { id = ban.Id }, ban);
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Host")]
        [HttpPut("add")]
        public async Task<IActionResult> CreateBan([Bind("Id", "BanExpiryTime", "SubjectIPAddress", "SubjectCkey", "SubjectCid", "AdminCkey", "BanType", "Job", "Reason")][FromBody] Ban ban)
        {
            if (ban == null)
                return BadRequest();

            await _banRepository.Add(ban);
            return CreatedAtRoute("GetBan", new { id = ban.Id }, ban);
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Host")]
        [HttpPatch("update")]
        public async Task<IActionResult> UpdateBan([FromBody] Ban ban)
        {
            if (ban == null)
                return BadRequest();

            var exban = await _banRepository.FindByIdAsync(ban.Id);
            if (exban == null)
                return NotFound(new ErrorDTO(404, "The ban with specified id could not be found"));

            _banRepository.Update(ban);
            return CreatedAtRoute("GetBan", new { id = ban.Id }, ban);
        }

        [Authorize(ActiveAuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme, Roles = "Host")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveBan(int id)
        {
            var ban = await _banRepository.FindByIdAsync(id);

            if(ban == null)
            {
                return NotFound(new ErrorDTO(404, "The ban with specified id could not be found"));
            }

            _banRepository.Remove(id);

            return NoContent();
        }
    }
}
