// <copyright file="LocalAuthenticationController.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using Microsoft.AspNet.Mvc;

namespace AuthorIntrusion.Web.Plugin.LocalAuthentication.Controllers
{
	public class LocalAuthenticationController : Controller
	{
		#region Public Methods and Operators

		public IActionResult Index()
		{
			return View();
		}

		#endregion
	}
}
