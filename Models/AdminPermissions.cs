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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Models
{
    // These are used in game server itself, not here

    /// <summary>
    ///     Permission flags used for accessing various hub services. Compatible with game build.
    /// </summary>
    [Flags]
    public enum AdminPermissions
    {
        ///<summary>Null flag</summary>
        R_NOTHING       =    0,
        R_BUILDMODE     =    0x1,
        R_ADMIN         =    0x2,
        R_BAN           =    0x4,
        R_FUN           =    0x8,
        R_SERVER        =    0x10,
        R_DEBUG         =    0x20,
        R_POSSESS       =    0x40,
        R_PERMISSIONS   =    0x80,
        R_STEALTH       =    0x100,
        R_REJUVINATE    =    0x200,
        R_VAREDIT       =    0x400,
        R_SOUNDS        =    0x800,
        R_SPAWN         =    0x1000,
        R_MOD           =    0x2000,
        R_MENTOR        =    0x4000,
        R_HOST          =    0x8000,
        R_INVESTIGATE   =    (R_ADMIN|R_MOD),
        R_EVERYTHING    =    (R_BUILDMODE|R_ADMIN|R_BAN|R_FUN|R_SERVER|R_DEBUG|R_POSSESS|R_STEALTH|R_REJUVINATE|R_VAREDIT|R_SOUNDS|R_SPAWN|R_MOD|R_MENTOR|R_HOST)
    }
}
