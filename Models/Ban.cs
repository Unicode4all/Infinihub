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
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Models
{
    /// <summary>
    /// DTO for ban persistence
    /// </summary>
    public class Ban
    {
        public int Id { get; set; }

        /// <summary>
        /// Specifies date when a new ban added.
        /// </summary>
        /// <value>Ban date</value>
        [Display(Name = "Дата бана")]
        public DateTime? BanDate { get; set; }

        /// <summary>
        /// Used for informational purpose
        /// </summary>
        /// <value>Ban duration in minutes</value>
        [Display(Name = "Срок бана в минутах")]
        public long BanExpiration { get; set; }

        /// <summary>
        /// Date when ban expires
        /// </summary>
        /// <value>Expiration date</value>
        public DateTime? BanExpirationDate { get; set; }

        /// <summary>
        /// Additional player identifier if banned player has created a new account.
        /// </summary>
        /// <value>IPv4 address string.</value>
        [Display(Name = "IP Адрес")]
        public string SubjectIPAddress { get; set; }

        /// <summary>
        /// Primary player identifier
        /// </summary>
        /// <value>Client BYOND login string.</value>
        [Required]
        [Display(Name = "BYOND Ckey")]
        public string SubjectCkey { get; set; }

        /// <summary>
        /// The last line of defense from banned player which using any of IP-change method such as VPN, proxy or Tor.
        /// <note type="note">Not very useful if client is using virtual machine as their hardware config will be different and their ID respectively.</note>
        /// </summary>
        /// <value>Unique computer ID generated from client hardware configuration.</value>
        [Display(Name = "Computer ID")]
        public string SubjectCid { get; set; }
        /// <summary>
        /// BYOND login of those who banned this poor player.
        /// </summary>
        /// <value>BYOND login string</value>
        [Display(Name = "Ckey админа")]
        public string AdminCkey { get; set; }

        /// <summary>
        /// Determines ban type. 
        /// </summary>
        /// <value>Ban type enum. See <see cref="Infinihub.Models.BanType"/> for available ban types.</value>
        [Required]
        [Display(Name = "Тип бана")]
        public BanType? BanType { get; set; }

        /// <summary>
        /// Banned job if jobban, empty if generic ban.
        /// </summary>
        /// <value>Job name</value>
        [Display(Name = "Профессия")]
        public string Job { get; set; }

        /// <summary>
        /// Ban reason
        /// </summary>
        /// <value>Reason string</value>
        [Required]
        [MinLength(10)]
        [Display(Name = "Причина")]
        public string Reason { get; set; }
    }
}
