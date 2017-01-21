using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Infinihub.Models.OAuthViewModels
{
    public class AuthorizeViewModel
    {
        [Display(Name = "Приложение")]
        public string ApplicationName { get; set; }

        [BindNever]
        public string RequestId { get; set; }

        [Display(Name = "Разрешение")]
        public string Scope { get; set; }
    }
}
