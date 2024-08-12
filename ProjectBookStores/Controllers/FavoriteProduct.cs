using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using System.Security.Claims;

namespace ProjectBookStores.Controllers
{

    [Route("favoriteProduct")]
    public class FavoriteProduct : Controller
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FavoriteProduct(IHttpContextAccessor httpContextAccessor, ProjectBookStoreContext db)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("listFavoriteProduct/{id}")]
        public IActionResult listFavoriteProduct(long id)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Sign-in", "Customer");
            }
            var dao = new bookFavoriteDao();
            var model = dao.listFavorites(id);
            var faver = _db.BookFaverites.FirstOrDefault();
            ViewBag.Faver = faver;
            return View(model);
        }

        [Route("insertFavorite")]
        [HttpPost]
        public IActionResult listFavorites(BookFaverite favoriteProd)
        {
            if(ModelState.IsValid)
            {
                var dao = new bookFavoriteDao();
                long id = dao.insertFavorite(favoriteProd);
                var userId = GetCurrentUserId();
                if (id < 0)
                {
                    ModelState.AddModelError("", "Thêm không thành công");
                    
                }
                else
                {
                    ModelState.AddModelError("", "Thêm thành công");
                    return RedirectToAction("listFavoriteProduct", "FavoriteProduct", new { id = userId });
                }
            }
            return View(favoriteProd);
            
        }

        public IActionResult Delete(long id)
        {
            var userId = GetCurrentUserId();
            new BookDao().DeleteBookFaverites(id);
            return RedirectToAction("listFavoriteProduct", "FavoriteProduct", new { id = userId });
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