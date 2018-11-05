using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TelegrammAspMvcDotNetCoreBot.Models;
using Microsoft.EntityFrameworkCore;

namespace TelegrammAspMvcDotNetCoreBot.Controllers
{

	public class HomeController : Controller
	{
		private MyContext db;
		
		public HomeController(MyContext context)
		{
			db = context;
		}

		public IActionResult Index()
        {
			University un = new University();
			un.Name = "rtre";

			db.Universities.Add(un);

			db.SaveChanges();

			ViewBag.n = db.Universities.Where(n => n.Name == "rtre").FirstOrDefault().Id;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
