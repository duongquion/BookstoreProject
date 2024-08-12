using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using System.Security.Claims;

namespace ProjectBookStores.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomerController(ProjectBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult listOrder()
        {
            var dao = new OrderDao();
            var userId = GetCurrentUserId();
            var model = dao.GetAllOrders().Where(x => x.CustomerId == userId);
            return View(model);
        }

        public IActionResult orderDetail(long id, string code)
        {
            var dao = new OrderDao();
            var model = dao.detailOder(id, code);
            return View(model);
        }

        public IActionResult Infomation()
        {
            var userId = GetCurrentUserId();
            var model = _db.Customers.FirstOrDefault(x => x.Id == userId);
            return View(model);
        }

        public IActionResult deleteOrder(long id)
        {
            new OrderDao().DeleteOrder(id);
            return RedirectToAction("allProduct", "Product");
        }

        public IActionResult questionDelete()
        {
            var order = _db.Orders.Select(o => new { o.Id, o.Code }).FirstOrDefault();
            ViewBag.Order = order;
            return View();
        }

        private long? GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return null;
        }
    }
}
