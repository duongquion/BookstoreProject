using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;

public class BaseController : Controller
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
		if (!isAuthenticated && !context.HttpContext.Request.Path.Value.Contains("login"))
		{
			context.Result = RedirectToAction("Login", "Identity", new { area = "Admin" });
		}
		else
		{
			base.OnActionExecuting(context);
		}
	}
}
