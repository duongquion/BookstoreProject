using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Common;
using ProjectBookStores.Controllers;
using ProjectBookStores.EF;
using ProjectBookStores.Models;
using System.Security.Claims;

namespace ProjectBookStores.Components
{
    public class countCartViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public countCartViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public IViewComponentResult Invoke()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                const string CART_KEY = "cart";
                var cart = HttpContext.Session.Get<List<CartModel>>(CART_KEY) ?? new List<CartModel>();
                var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var count = cart.Count(x => x.cusId == userId);
                return View(count);
            }
            return View(0);
        }
    }
}
