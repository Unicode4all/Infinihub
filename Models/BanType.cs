﻿//   Copyright 2017 Solaris 13 Foundation
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Models
{
    /// <summary>
    /// An enum used to specify ban type.
    /// </summary>
    public enum BanType
    {
        /// <summary>
        /// Temporary ban
        /// </summary>
        TEMP,
        /// <summary>
        /// Permanent ban
        /// </summary>
        PERMABAN,
        /// <summary>
        /// Job temporary ban
        /// </summary>
        JOBTEMPBAN,
        /// <summary>
        /// Job permanent ban
        /// </summary>
        JOBPERMABAN,
        /// <summary>
        /// Ban specified character from being selected during game startup.
        /// </summary>
        CHARACTER
    }
}
