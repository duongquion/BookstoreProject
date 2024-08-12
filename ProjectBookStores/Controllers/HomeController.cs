using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;

namespace ProjectBookStores.Controllers;
public class HomeController : Controller
{
    private readonly ProjectBookStoreContext _db;
    public HomeController (ProjectBookStoreContext db)
    {
        _db = db;
    }
    public IActionResult Index()
    {
        var dao = new BookDao();
        var model = dao.bookBanner();

        var code = _db.Orders.Select(o => o.Code).FirstOrDefault();
        ViewBag.Code = code;

        var saleProduct = dao.SaleBook();
        ViewBag.SaleProduct = saleProduct;

        var hotProduct = dao.HotBook();
        ViewBag.HotProduct = hotProduct;

        var newProduct = dao.NewBook();
        ViewBag.NewProduct = newProduct;
        return View(model);
    }
}
