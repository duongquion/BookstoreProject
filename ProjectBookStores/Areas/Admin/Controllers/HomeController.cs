using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Areas.Admin.Models;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using ProjectBookStores.ViewModels;
using System.Globalization;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ProjectBookStores.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class HomeController : BaseController
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public HomeController(ProjectBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();

            var delivery = _db.Orders.Join(_db.OrderStatuses,
              order => order.OrderStatusId,
              status => status.Id,
              (order, status) => new { Order = order, Status = status }).Count(os => os.Status.StatusName == "Đã giao hàng");
            ViewBag.Delivery = delivery;

            var toltalPrice = _db.OrderDetails.Sum(x => x.TotalPrice);
            ViewBag.TotalPrice = toltalPrice;

            var customer = _db.Customers.Select(x => x.Id).Count();
            ViewBag.Customer = customer;

            var order = _db.Orders.Select(x => x.Id).Count();
            ViewBag.Order = order;

			//Chart.js
			var chartDao = new AdminDao();
			var monthlyPriceCount = chartDao.GetOrderChart();

			var chartData = new
			{
				Labels = monthlyPriceCount.Select(entry => $"{CultureInfo.GetCultureInfo("vi-VN").DateTimeFormat.GetMonthName(entry.Month)} {entry.Year}").ToList(),
				Data = monthlyPriceCount.Select(entry => entry.NumberOfPrice).ToList()
			};

			var chartDataJson = JsonSerializer.Serialize(chartData, new JsonSerializerOptions
			{
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase
			});

			ViewBag.ChartDataJson = chartDataJson;

			var dao = new OrderDao();
            var model = dao.GetAllOrders();
            return View(model);
        }

        private long? GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return null;
        }

        [HttpPost]
        [Route("ToggleOrderStatu/{id}")]
        public IActionResult ToggleOrderStatus(long Id)
        {
            var dao = new OrderDao();
            bool result = dao.AccessOrder(Id);

            if (result)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Không thể cập nhật trạng thái đơn hàng.");
                return RedirectToAction("Index");
            }
        }

        [Route("list")]
        public IActionResult ListAdmin(string searchString)
        {
            var dao = new AdminDao();
            var model = dao.GetAll(searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [Route("AdminProfile/{id?}")]
        [HttpGet("AdminProfile")]
        public IActionResult AdminProfile(long id)
        {
            var dao = new AdminDao().GetAdmin(id);
            ViewBag.Id = dao;
            return View();
        }

        [HttpPost("AdminProfile")]
        public IActionResult AdminProfile(EF.Admin admin)
        {
            if (ModelState.IsValid)
            {
                var dao = new AdminDao();
                admin.CreateDate = DateTime.Now;
                admin.CreateBy = "Quản trị viên";
                admin.Status = true;
                long id = dao.Insert(admin);
                if (id > 0)
                {
                    ModelState.AddModelError("", "Cap nhat that bai!");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Cap nhat thanh cong!");
                }
            }
            return View("AdminProfile");
        }
    }
}
