using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Trains.Infrastructure.Interfaces;

namespace Trains.Web.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

		public HomeController(IAppSettings appSettings)
		{
			var a = appSettings;
		}
	}
}
