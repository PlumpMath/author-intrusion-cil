// <copyright file="Startup.cs" company="Moonfire Games">
//   Copyright (c) Moonfire Games. Some Rights Reserved.
// </copyright>
// <license href="http://mfgames.com/mfgames-cil/license">
//   MIT License (MIT)
// </license>

using AuthorIntrusion.Web.Properties;

using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Diagnostics;
using Microsoft.AspNet.Hosting;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Logging;

namespace AuthorIntrusion.Web
{
	public class Startup
	{
		#region Constructors and Destructors

		public Startup(IHostingEnvironment env)
		{
			// Setup configuration sources.
			Configuration = new Configuration()
				.AddJsonFile("config.json")
				.AddEnvironmentVariables();
		}

		#endregion

		#region Public Properties

		public IConfiguration Configuration { get; set; }

		#endregion

		#region Public Methods and Operators

		// Configure is called after ConfigureServices is called.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			ILoggerFactory loggerfactory)
		{
			// Configure the HTTP request pipeline.

			// Add the console logger.
			loggerfactory.AddConsole();

			// Add the following to the request pipeline only in development environment.
			if (env.IsEnvironment("Development"))
			{
				app.UseBrowserLink();
				app.UseErrorPage(ErrorPageOptions.ShowAll);
			}
			else
			{
				// Add Error handling middleware which catches all application specific errors and
				// send the request to the following path or controller action.
				app.UseErrorHandler("/Home/Error");
			}

			// Add static files to the request pipeline.
			app.UseStaticFiles();

			// Add MVC to the request pipeline.
			app.UseMvc(
				routes =>
					{
						routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
					});
		}

		// This method gets called by the runtime.
		public void ConfigureServices(IServiceCollection services)
		{
			services.Configure<AppSettings>(Configuration.GetSubKey("AppSettings"));

			// Add MVC services to the services container.
			services.AddMvc();
		}

		#endregion
	}
}
