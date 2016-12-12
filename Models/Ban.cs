using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Infinity.so.Models
{
    public class Ban
    {
        public int Id { get; set; }

        [Display(Name = "Дата бана")]
        public DateTime? BanDate { get; set; }
        [Display(Name = "Дата истечения")]
        public DateTime? BanExpiryTime { get; set; }
        [Display(Name = "IP Адрес")]
        public string SubjectIPAddress { get; set; }

        // Subject's BYOND Login
        [Required]
        [Display(Name = "BYOND Ckey")]
        public string SubjectCkey { get; set; }

        // Subject's Computer ID
        [Display(Name = "Computer ID")]
        public string SubjectCid { get; set; }
        [Display(Name = "Ckey админа")]
        public string AdminCkey { get; set; }

        [Required]
        [Display(Name = "Тип бана")]
        public BanType? BanType { get; set; }
        [Display(Name = "Профессия")]
        public string Job { get; set; }

        [Required]
        [MinLength(10)]
        [Display(Name = "Причина")]
        public string Reason { get; set; }
    }
}
