using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Infinity.so.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Route("/error/{code}")]
        public IActionResult Error(int code)
        {
            switch (code)
            {
                case 404:
                    return View("Error/404", Request.Path);
                case 500:
                    return View("Error/500");
            }
            return View(code);
        }

        [Route("/testerr")]
        public IActionResult SampleError()
        {
            throw new Exception("Test");
            return Content("Test");
        }
    }
}
