using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Portal_Sisgen.Areas.User.Controllers
{
    [Area("Client")]
    public class CesionesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}