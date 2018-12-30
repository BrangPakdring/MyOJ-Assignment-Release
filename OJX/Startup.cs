using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Services.Description;
using Microsoft.Owin;
using Microsoft.Owin.Security.Google;
using Owin;

[assembly: OwinStartup(typeof(OJX.Startup))]

namespace OJX
{
	public partial class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}
	}
}
