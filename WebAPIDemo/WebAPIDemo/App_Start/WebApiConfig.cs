using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebAPIDemo
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();//attribute  route

			config.Routes.MapHttpRoute(//傳統的route
				name: "DefaultApi" ,
				routeTemplate: "api/{controller}/{id}" ,
				defaults: new { id = RouteParameter.Optional }
			);
		}
	}
}