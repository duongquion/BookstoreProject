using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectBookStores.Dao;
using ProjectBookStores.EF;
using ProjectBookStores.ViewModels;
using System.Drawing;
using System.Xml.Linq;

namespace ProjectBookStores.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("admin")]
    public class ProductController : Controller
    {
        private readonly ProjectBookStoreContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IWebHostEnvironment webHostEnvironment, ProjectBookStoreContext db)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        [Route("dssp")]
        public IActionResult Index(string searchString)
        {
            var dao = new BookDao();
            var model = dao.GetAll(searchString);
            ViewBag.SearchString = searchString;
            return View(model);
        }

        [Route("create")]
        [HttpGet]
        public IActionResult Create()
        {
			var genreList = new bookGenreDao().genreList() ?? new List<BooksGenre>();
			var publisherList = new PublisherDao().getPublisherList() ?? new List<Publisher>();

			ViewBag.GenreId = new SelectList(genreList, "Id", "Name");
			ViewBag.PublisherID = new SelectList(publisherList, "Id", "PublisherName");
			return View();
        }

        [Route("create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Book book, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var webRootPath = _webHostEnvironment.WebRootPath;

                if (files != null && files.Count > 0)
                {
                    var mainImage = files[0];
                    var mainImagePath = Path.Combine(webRootPath, "Assets/Product/img", mainImage.FileName);

                    using (var stream = new FileStream(mainImagePath, FileMode.Create))
                    {
                        mainImage.CopyTo(stream);
                    }
                    book.Images = Path.Combine("/Assets/Product/img", mainImage.FileName);

                    var moreImages = new List<string>();
                    for (int i = 1; i < files.Count; i++)
                    {
                        var file = files[i];
                        var filePath = Path.Combine(webRootPath, "Assets/Product/img", file.FileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        moreImages.Add(Path.Combine("/Assets/Product/img", file.FileName));
                    }

                    // Convert the list of more images to XML
                    var moreImagesXml = new XDocument(
                        new XElement("Images",
                            moreImages.Select(img => new XElement("Image", img))
                        )
                    );
                    book.MoreImages = moreImagesXml.ToString();
                }

                var dao = new BookDao();
                book.CreateDate = DateTime.Now;
                book.Status = true;
                long id = dao.AddBook(book);
                if (id > 0)
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm sách thất bại");
                }
            }

            var genreList = new bookGenreDao().genreList() ?? new List<BooksGenre>();
            var publisherList = new PublisherDao().getPublisherList() ?? new List<Publisher>();

            ViewBag.GenreId = new SelectList(genreList, "Id", "Name");
            ViewBag.PublisherId = new SelectList(publisherList, "Id", "PublisherName");

            return View(book);
        }


        [Route("edit")]
        [HttpGet]
        public IActionResult Edit(long id)
        {
            var book = new BookDao().GetBook(id);

			var genreList = new bookGenreDao().genreList() ?? new List<BooksGenre>();
			var publisherList = new PublisherDao().getPublisherList() ?? new List<Publisher>();

			ViewBag.GenreID = new SelectList(genreList, "Id", "Name");
			ViewBag.PublisherID = new SelectList(publisherList, "Id", "PublisherName");


			return View(book);
        }
        [Route("edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dao = new BookDao();
                    if (files != null && files.Count > 0)
                    {
                        var webRootPath = _webHostEnvironment.WebRootPath;

                        var mainImage = files.First();
                        var mainImagePath = Path.Combine(webRootPath, "Assets/Product/img", mainImage.FileName);

                        if (!string.IsNullOrEmpty(book.Images))
                        {
                            var oldImagePath = Path.Combine(webRootPath, "Assets/Product/img", Path.GetFileName(book.Images));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        try
                        {
                            using (var stream = new FileStream(mainImagePath, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                await mainImage.CopyToAsync(stream);
                            }
                            book.Images = Path.Combine("/Assets/Product/img", mainImage.FileName);
                        }
                        catch (IOException ex)
                        {
                            ModelState.AddModelError("", $"Lỗi không thể tải hình ảnh: {ex.Message}");
                        }

                        var moreImages = new List<string>();
                        for (int i = 1; i < files.Count; i++)
                        {
                            var file = files[i];
                            var filePath = Path.Combine(webRootPath, "Assets/Product/img", file.FileName);

                            try
                            {
                                using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    await file.CopyToAsync(stream);
                                }
                                moreImages.Add(Path.Combine("/Assets/Product/img", file.FileName));
                            }
                            catch (IOException ex)
                            {
                                ModelState.AddModelError("", $"Lỗi không thể tải hình: {ex.Message}");
                            }
                        }
                        book.MoreImages = string.Join(";", moreImages);
                    } 
                    bool result = dao.UpdateBook(book);
                    if (result)
                    {
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật không thành công");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật sản phẩm: " + ex.Message);
                }
            }
			var genreList = new bookGenreDao().genreList() ?? new List<BooksGenre>();
			var publisherList = new PublisherDao().getPublisherList() ?? new List<Publisher>();

			ViewBag.GenreID = new SelectList(genreList, "Id", "Name");
			ViewBag.PublisherID = new SelectList(publisherList, "Id", "PublisherName");

			return View(book);
        }

        [Route("delete/{id?}")]
        public IActionResult Delete(long id)
        {
            new BookDao().DeleteBook(id);
            return RedirectToAction("Index");
        }

        [Route("dmlsp")]
        public IActionResult bookgenreList()
        {
            var dao = new bookGenreDao();
            var model = dao.genreList();
            return View(model);
        }
        [Route("create-dmsp")]
        [HttpGet]
        public IActionResult createGenre()
        {
            return View();
        }
        [Route("create-dmsp")]
        [HttpPost]
        public IActionResult createGenre(BooksGenre genre)
        {
            if(ModelState.IsValid)
            {
                var dao = new bookGenreDao();
                genre.Status = true;
                long id = dao.addGenre(genre);
                if(id < 0)
                {
                    ModelState.AddModelError("", "Thêm danh mục thất bại!");
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm danh mục thành công!");
                }
            }
            return View(genre);
        }
        [Route("genreDelete")]
        public IActionResult genreDelete(long id)
        {
            new bookGenreDao().deleteGenre(id);
            return RedirectToAction("Index");
        }

        
    }
}
