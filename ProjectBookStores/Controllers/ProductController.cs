using Humanizer.Localisation;
using Microsoft.AspNetCore.Mvc;
using ProjectBookStores.Common;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using System.Runtime.CompilerServices;
using X.PagedList;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProjectBookStores.Controllers
{
    [Route("Product")]
    public class ProductController : Controller
    {
        [Route("allProduct")]
        public IActionResult allProduct(string sortOrder, string language, string genre, string sortPrice, string searchString, int? page)
        {
            var dao = new BookDao();
            var model = dao.getAllbook();

            ViewBag.Genres = model.Select(x => x.GenresName).Distinct().ToList();   
            ViewBag.Language = model.Select(x => x.Language).Distinct().ToList();

            if (!string.IsNullOrEmpty(genre))
            {
                model = model.Where(b => b.GenresName.Equals(genre));
            }
            ViewBag.CurrentGenre = genre;

            if (!string.IsNullOrEmpty(language))
            {
                model = model.Where(b => b.Language.Equals(language));
            }
            ViewBag.CurrentLanguage = language;

            if (!string.IsNullOrEmpty(searchString))
            {
                var normalizedSearchString = searchString.RemoveDiacritics().ToLower();
                model = model.Where(x => x.ProName.RemoveDiacritics().ToLower().Contains(normalizedSearchString) ||
                                         x.AuthorName.RemoveDiacritics().ToLower().Contains(normalizedSearchString)).ToList();
            }
            ViewBag.CurrentSearchString = searchString;

            switch (sortOrder)
            {
                case "Newest":
                    model = model.OrderByDescending(x => x.CreateDate);
                    break;
                case "Oldest":
                    model = model.OrderBy(x => x.CreateDate);
                    break;
                case "Name_asc":
                    model = model.OrderBy(b => b.ProName);
                    break;
                case "Name_desc":
                    model = model.OrderByDescending(b => b.ProName);
                    break;
                default:
                    model = model.OrderBy(x => x.Price);
                    break;
            }
            ViewBag.CurrentSortOrder = sortOrder;

            switch (sortPrice)
            {
                case "Under100VND":
                    model = model.Where(x => x.Price < 100000).OrderByDescending(x => x.CreateDate); 
                    break;
                case "Between100VNDand250VND":
                    model = model.Where(x => x.Price >= 100000 && x.Price < 250000).OrderByDescending(x => x.CreateDate);
                    break;
                case "Between250VNDand500VND":
                    model = model.Where(x => x.Price >= 250000 && x.Price < 500000).OrderByDescending(x => x.CreateDate);
                    break;
                case "Over500VND":
                    model = model.Where(x => x.Price > 500000).OrderByDescending(x => x.CreateDate);
                    break;
                default:
                    model = model.OrderBy(x => x.CreateDate);
                    break;
            }
            ViewBag.CurrentSortPrice = sortPrice;

            return View(model.ToPagedList(page ?? 1, 20));
        }

        [Route("detailProduct/{id}/{genid}")]
        public IActionResult detailProduct(long id, long genid)
        {
            var daoBook = new BookDao();
            var daoGenre = new bookGenreDao();

            var book = daoBook.GetBook(id);
            ViewBag.Book = book;
            var genre = daoGenre.getGenre(genid);
            ViewBag.Genre = genre;
            return View();
        }
    }
}
