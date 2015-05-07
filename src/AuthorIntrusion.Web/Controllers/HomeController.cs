// <copyright file="HomeController.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using Microsoft.AspNet.Mvc;

namespace AuthorIntrusion.Web.Controllers
{
	public class HomeController : Controller
	{
		#region Public Methods and Operators

		public IActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View("~/Views/Shared/Error.cshtml");
		}

		public IActionResult Index()
		{
			return View();
		}

		#endregion
	}
}
