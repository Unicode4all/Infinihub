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
using Infinihub.Models.AccountViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Controllers.Api
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class HubTestController : Controller
    {
        // For testing within game build. Build will send test JSON on this endpoint and receive echo of the request.
        [HttpPost]
        public IActionResult Test([FromBody] ApiTestData data)
        {
            if (data.Header == "HubTest" && data.Test == "{7b01d3b6-778c-4bb3-bb37-7a119abbfcc0}")
                return Json(data);

            return BadRequest();
        }
    }
}
