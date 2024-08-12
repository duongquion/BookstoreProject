using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Areas.Admin.Models;
using ProjectBookStores.Dao;
using System.Security.Claims;
using System.Text.Json;

namespace ProjectBookStores.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Route("admin")]
	public class Identity : Controller
	{
		[Route("login")]
		[HttpGet("Login")]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				var dao = new AdminDao();
				var result = dao.Login(model.Email, model.Password);
				if (result == 1)
				{
					var admin = dao.GetAdminById(model.Email);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, admin.UserName),
						new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
					};
					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
					var claimsPrinciple = new ClaimsPrincipal(claimsIdentity);
					await HttpContext.SignInAsync(claimsPrinciple);
					return RedirectToAction("Index", "Home");
				}
				else if (result == 0)
				{
					ModelState.AddModelError("", "Tài khoản không tồn tại!");
				}
				else if (result == -2)
				{
					ModelState.AddModelError("", "Đăng nhập thất bại");
				}
				else if (result == -1)
				{
					ModelState.AddModelError("", "Tài khoản đã bị khóa!");
				}
			}
			return View(model);
		}

		[Route("logout")]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Login", "Identity");
		}

		[Route("register")]
		[HttpGet("Register")]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost("Register")]
		public IActionResult Register(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				var dao = new AdminDao();
				if (dao.CheckAdmin(model.email))
				{
					ModelState.AddModelError("", "Tài khoản đã tồn tại!");
				}
				else
				{
					var admin = new EF.Admin();
					admin.UserName = model.username.Trim();
					admin.PassWord = model.password.Trim();
					admin.Email = model.email.Trim();
					admin.CreateDate = DateTime.Now;
					admin.CreateBy = "Quản trị viên";
					admin.Status = true;
					var result = dao.Insert(admin);
					if (result > 0)
					{
						model = new RegisterModel();
						return RedirectToAction("Login", "Identity");
					}
					else
					{
						ModelState.AddModelError("", "Đăng ký thất bại!");
					}
				}
			}
			return View(model);
		}
        [HttpPost]
        [Route("changeStatus/{id}")]
        public IActionResult ChangeStatus(long id)
        {
            var result = new AdminDao().ChangeStatus(id);
            return Json(new
            {
                status = result
            });
        }
    }
}
