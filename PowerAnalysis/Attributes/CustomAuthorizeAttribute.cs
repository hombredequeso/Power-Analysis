﻿using System;

namespace HDC.PowerAnalysis.Web.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class CustomAuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
	{
		protected override void HandleUnauthorizedRequest(System.Web.Mvc.AuthorizationContext filterContext)
		{
			if (filterContext.HttpContext.Request.IsAuthenticated)
			{
				filterContext.Result = new System.Web.Mvc.HttpStatusCodeResult((int)System.Net.HttpStatusCode.Forbidden);
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}