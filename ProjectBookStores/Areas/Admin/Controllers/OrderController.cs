using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using System.Security.Claims;

namespace ProjectBookStores.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class OrderController : Controller
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderController(ProjectBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("donhang")]
        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            var permissionId = (from admin in _db.Admins
                                join permission in _db.Permissions
                                on admin.PermissionId equals permission.Id
                                where admin.Id == userId
                                select permission.Id).FirstOrDefault();
            ViewBag.PermissionId = permissionId;
            var model = _db.Orders.Where(x => x.StatusOrder == true).ToList();
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

        public IActionResult customerOrderDetail(long id, string code)
        {
            var dao = new OrderDao();
            var model = dao.detailOder(id, code).FirstOrDefault();
            return View(model);
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(long orderId, byte statusId)
        {
            var dao = new OrderDao();
            var result = dao.UpdateOrderStatus(orderId, statusId);

            if (result)
            {
                return RedirectToAction("customerOrderDetail", new { id = orderId });
            }
            else
            {
                ModelState.AddModelError("", "Cập nhật thất bại");
                return RedirectToAction("customerOrderDetail", new { id = orderId });
            }
        }
    }
}
