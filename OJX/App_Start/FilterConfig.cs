﻿using System.Web;
using System.Web.Mvc;

namespace OJX
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
//			if (!HttpContext.Current.IsDebuggingEnabled)
//			filters.Add(new RequireHttpsAttribute());
		}
	}
}