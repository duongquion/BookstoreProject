using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using System.Security.Claims;

namespace ProjectBookStores.Components
{
    public class customerAddressViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public customerAddressViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IViewComponentResult Invoke()
        {
            var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var dao = new OrderDao();
            var model = dao.GetAddress(userId);
            return View(model);
        }
    }
}
