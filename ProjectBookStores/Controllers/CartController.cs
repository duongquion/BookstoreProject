using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectBookStores.Common;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using ProjectBookStores.Models;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace ProjectBookStores.Controllers
{
    public class CartController : Controller
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(ProjectBookStoreContext context, IHttpContextAccessor httpContextAccessor)
        {
            _db = context;
            _httpContextAccessor = httpContextAccessor;
        }

        const string CART_KEY = "cart";

        public List<CartModel> GetCartItems => HttpContext.Session.Get<List<CartModel>>(CART_KEY) ?? new List<CartModel>();

        public IActionResult Index()
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Sign-in", "Customer");
            }
            var cartItems = GetCartItems.Where(x => x.cusId == userId).ToList();
            return View(cartItems);
        }

        public IActionResult addToCart(long id, int quantity)
        {
            var userId = GetCurrentUserId();
            if (userId == null)
            {
                return RedirectToAction("Sign-in", "Customer");
            }

            var order = _db.Orders.Select(o => new { o.Id}).FirstOrDefault();
            ViewBag.Order = order;

            var cart = GetCartItems;
            var item = cart.SingleOrDefault(x => x.proId == id && x.cusId == userId);
            if (item == null)
            {
                var product = _db.Books.SingleOrDefault(x => x.Id == id);
                if (product == null)
                {
                    return NotFound("Không có sản phẩm");
                }
                item = new CartModel
                {
                    proId = product.Id,
                    proName = product.Name,
                    proImage = product.Images ?? string.Empty,
                    proQuantity = quantity,
                    proPrice = product.Price ?? 0,
                    cusId = (long)userId
                };
                cart.Add(item);
            }
            else
            {
                item.proQuantity += quantity;
            }

            saveCartSession(cart);
            HttpContext.Session.Set(CART_KEY, cart);

            return RedirectToAction("Index");
        }

        public IActionResult removeCart(long id)
        {
            var cart = GetCartItems;
            var item = cart.SingleOrDefault(x => x.proId == id);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.Set(CART_KEY, cart);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCartQuantity(long proId, long cusId, int quantity)
        {
            var cart = GetCartItems;
            var item = cart.SingleOrDefault(x => x.proId == proId && x.cusId == cusId);
            if (item != null)
            {
                item.proQuantity = quantity;
                saveCartSession(cart);
            }
            saveCartSession(cart);
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Checkout()
        {
            var userId = GetCurrentUserId();
            if (userId != null)
            {
                var userAddress = _db.Addresses.FirstOrDefault(a => a.CustomerId == userId);
                ViewBag.CheckAddress = userAddress;

                var userDiscount = _db.CustomerDiscounts.Where(x => x.CustomerId == userId).ToList();
                ViewBag.UserDiscount = userDiscount;

                if (GetCartItems.Count < 1)
                {
                    return NotFound("Khong co san pham nao!");
                }
            }
            var cartItems = GetCartItems.Where(x => x.cusId == userId).ToList();
            return View(cartItems);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Checkout(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var customerID = GetCurrentUserId();
                var order = new EF.Order
                {
                    CustomerId = customerID,
                    OrderDate = DateTime.Now,
                    OrderDelivery = DateTime.Now.AddDays(4),
                    Note = model.noteOrder,
                    StatusOrder = false,
                    Code = randomCode.GenerateRandomCode(30),
                    OrderStatusId = 1
                };
                _db.Database.BeginTransaction();
                try
                {
                    _db.Database.CommitTransaction();
                    _db.Add(order);
                    _db.SaveChanges();

                    var orderDetail = new List<OrderDetail>();
                    foreach (var item in GetCartItems)
                    {
                        orderDetail.Add(new OrderDetail
                        {
                            OrderId = order.Id,
                            BookId = item.proId,
                            Quantity = item.proQuantity,
                            UnitPrice = item.proPrice,
                            TotalPrice = item.finalPrice
                        });
                    }
                    _db.AddRange(orderDetail);
                    _db.SaveChanges();

                    HttpContext.Session.Set<List<CartModel>>(CART_KEY, new List<CartModel>());

                    ClearCart();

                    return RedirectToAction("Success", "Cart", new { id = order.Id });
                }
                catch
                {
                    _db.Database.RollbackTransaction();
                }
            }
            return View(GetCartItems);
        }

        public IActionResult Success(long id)
        {
            var code = _db.Orders.FirstOrDefault(x => x.Id == id);
            return View(code);
        }

        private long? GetCurrentUserId()
        {
            if (_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return long.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            return null;
        }

        void saveCartSession(List<CartModel> list)
        {
            var session = HttpContext.Session;
            string jsoncart = JsonConvert.SerializeObject(list);
            session.SetString(CART_KEY, jsoncart);
        }

        void ClearCart()
        {
            HttpContext.Session.Remove(CART_KEY);
        }

        [HttpGet]
        public IActionResult Address(string provinceId, string districtId, string wardId)
        {
            var userId = GetCurrentUserId();

            ViewBag.Provinces = _db.Provinces.ToList();
            ViewBag.Districts = _db.Districts.Where(d => string.IsNullOrEmpty(provinceId) || d.ProvinceCode == provinceId).ToList();
            ViewBag.Wards = _db.Wards.Where(w => string.IsNullOrEmpty(districtId) || w.DistrictCode == districtId).ToList();

            ViewBag.SelectedProvinceId = provinceId;
            ViewBag.SelectedDistrictId = districtId;
            ViewBag.SelectedWardId = wardId;

            var userAddress = _db.Addresses.FirstOrDefault(a => a.CustomerId == userId);
            ViewBag.CheckAddress = userAddress;

            var model = new AddressModel
            {
                ProvinceId = provinceId,
                DistrictId = districtId,
                WardId = wardId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult SaveAddress(AddressModel model)
        {
            var userId = GetCurrentUserId();

            if (ModelState.IsValid)
            {
                var address = new Address
                {
                    DetailAddress = model.detailAddress,
                    ProvinceId = model.ProvinceId,
                    DistrictId = model.DistrictId,
                    WardId = model.WardId,
                    CustomerId = (long)userId
                };

                _db.Addresses.Add(address);
                _db.SaveChanges();
                return RedirectToAction("Checkout", "Cart");
            }
            ViewBag.Provinces = _db.Provinces.ToList();
            ViewBag.Districts = _db.Districts.Where(d => string.IsNullOrEmpty(model.ProvinceId) || d.ProvinceCode == model.ProvinceId).ToList();
            ViewBag.Wards = _db.Wards.Where(w => string.IsNullOrEmpty(model.DistrictId) || w.DistrictCode == model.DistrictId).ToList();

            return View("Address", model);
        }

    }
}
