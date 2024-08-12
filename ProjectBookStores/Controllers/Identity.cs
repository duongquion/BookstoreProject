using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using System.Security.Claims;
using ProjectBookStores.Models;
using Microsoft.AspNetCore.Authorization;
using ProjectBookStores.EF;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using ProjectBookStores.Common;

namespace ProjectBookStores.Controllers
{
    [Route("Customer")]
    public class Identity : Controller
    {

        [Route("sign-in")]
        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                var result = dao.Login(model.email, model.password);
                if (result == 1)
                {
                    var customer = dao.GetCustomerById(model.email);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, customer.LastName + " " + customer.FirstName),
                        new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
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
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Identity");
        }

        [Route("sign-up")]
        [HttpGet("Register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var customer = new EF.Customer();
                var dao = new CustomerDao();
                if (dao.CheckCustomer(model.email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    customer.Email = model.email.Trim();
                    customer.PassWord = model.password.Trim();
                    customer.CreateDate = DateTime.Now;
                    customer.Status = true;
                    customer.PermissionId = 3;
                    var result = dao.Insert(customer);
                    if (result > 0)
                    {
                        ModelState.AddModelError("", "Đăng ký thành công!");
                        return RedirectToAction("Login", "Identity");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký thất bại");
                    }
                }
            }
            return View(model);
        }

        [Route("ForgotPassword")]
        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                var customer = dao.GetCustomerById(model.Email);
                if (customer == null)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại");
                    return View(model);
                }
                customer.ResetToken = Guid.NewGuid().ToString();
                customer.ResetTokenExpiration = DateTime.Now.AddHours(1);
                dao.Update(customer);
                var resetLink = Url.Action("ResetPassword", "Identity", new { token = customer.ResetToken, email = customer.Email }, Request.Scheme);
                await SendResetEmail(customer.Email, resetLink);

                return RedirectToAction("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordModel { Token = token, Email = email };
            return View(model);
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                var customer = dao.GetCustomerByResetToken(model.Token);
                if (customer == null || customer.ResetTokenExpiration < DateTime.Now)
                {
                    ModelState.AddModelError("", "Token hết hiệu lực");
                    return View(model);
                }

                customer.PassWord = model.password;
                customer.ResetToken = null;
                customer.ResetTokenExpiration = null;

                bool result = dao.Update(customer);
                if (result)
                {
                    return RedirectToAction("Login", "Identity");
                }
                else
                {
                    ModelState.AddModelError("", "Đổi mật khẩu thất bại!");
                }
            }

            return View(model);
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View("ForgotPasswordConfirmation");
        }

        //Lấy token từ URL
        private string GeneratePasswordResetToken()
        {
            return Guid.NewGuid().ToString();
        }

        //Cấu hình send Email
        private async Task SendResetEmail(string email, string link)
        {
            var mail = new MailMessage();
            mail.To.Add(email);
            mail.Subject = "Cập nhật mật khẩu";
            mail.Body = $"Ấn vào link để được cập nhật lại mật khẩu: {link}";
            mail.IsBodyHtml = true;
            mail.From = new MailAddress("quyonduong@gmail.com");

            using (var smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new System.Net.NetworkCredential("quyonduong@gmail.com", "bftq qvxh ywbt jewx");
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(mail);
            }
        }
    }
}