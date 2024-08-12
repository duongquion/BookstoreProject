using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBookStores.EF;
using System.Security.Claims;

namespace ProjectBookStores.ViewComponents
{
    public class countProductViewComponent : ViewComponent
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public countProductViewComponent(ProjectBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(long id)
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                var userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var count = await _db.BookFaverites.CountAsync(x => x.CustomerId == userId);
                return View(count);
            }
            else
            {
                return View(0);
            }
        }
    }
}
