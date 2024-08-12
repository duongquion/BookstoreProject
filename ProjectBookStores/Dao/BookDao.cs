using ProjectBookStores.EF;
using ProjectBookStores.ViewModels;
using System.Drawing;

namespace ProjectBookStores.Dao
{
    public class BookDao
    {
        ProjectBookStoreContext db = null;
        public BookDao()
        {
            db = new ProjectBookStoreContext();
        }

        #region Lấy danh sách theo nhu cầu ở trang Home
        public IEnumerable<Book> SaleBook()
        {
            return db.Books.Where(x => x.IntialPrice > 0).ToList();
        }

        public IEnumerable<Book> HotBook()
        {
            return db.Books.Where(x => x.Amount < 5).ToList();
        }

        public IEnumerable<Book> NewBook()
        {
            return db.Books.OrderByDescending(x => x.CreateDate).ToList();
        }
        #endregion

        #region Lấy sách theo ID
        public Book GetBook(long id)
        {
            return db.Books.Find(id);
        }
        #endregion

        public IEnumerable<BannerViewModel> bookBanner()
        {
            var model = (from a in db.Books
                         join b in db.BookRanks on a.Id equals b.BookId
                         select new BannerViewModel
                         {
                             proId = a.Id,
                             genreId = a.GenreId,
                             proAuthor = a.AuthorName,
                             proName = a.Name,
                             proBanner = b.Banner,
                             proDescription = a.Description,
                             proImages = a.Images,
                         }).ToList();
            return model;
        }

        #region Lấy toàn bộ sách
        //Phía người dùng sử dụng
        public IEnumerable<ProductViewModel> getAllbook()
        {
            var model = (from a in db.Books
                         join b in db.BooksGenres on a.GenreId equals b.Id
                         join c in db.Publishers on a.PublisherId equals c.Id
                         select new ProductViewModel
                         {
                            ProId = a.Id,
                            ProName = a.Name,
                            AuthorName = a.AuthorName,
                            IntialPrice = a.IntialPrice,
                            Price = a.Price,
                            Language = a.Language,
                            PublishingYear = a.PublishingYear,
                            Description = a.Description,
                            CreateDate = a.CreateDate,
                            Amount = a.Amount,
                            Images = a.Images,
                            GenreId = a.GenreId,
                            GenresName = b.Name,
                            PublisherId = a.PublisherId,
                            PublisherName = c.PublisherName,
                         }).ToList();
            return model;
        }
        //Phía Admin sẽ sử dụng
        public IEnumerable<Book> GetAll(string searchString)
        {
            IQueryable<Book> model = db.Books;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString)||x.AuthorName.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).Where(x => x.Status == true);
        }
        #endregion
        public bool DeleteBookFaverites(long id)
        {
            try
            {
                var book = db.BookFaverites.Find(id);
                db.BookFaverites.Remove(book);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Chức năng CRUD
        public long AddBook(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();
            return book.Id;
        }

        public bool DeleteBook(long id)
        {
            try
            {
                var book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdateBook(Book entity)
        {
            try
            {
                var book = db.Books.Find(entity.Id);
                book.Name = entity.Name;
                book.AuthorName = entity.AuthorName;
                book.IntialPrice = entity.IntialPrice;
                book.Price = entity.Price;
                book.Language = entity.Language;
                book.PublishingYear = entity.PublishingYear;
                book.Description = entity.Description;
                book.Status = true;
                book.Amount = entity.Amount;
                book.CreateDate = DateTime.Now;
                book.GenreId = entity.GenreId;
                book.MoreImages = entity.MoreImages;
                book.PublisherId = entity.PublisherId;
                db.Books.Update(book);
                // Lưu các thay đổi vào cơ sở dữ liệu
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                // Xử lý ngoại lệ nếu có
                return false;
            }
        }
        #endregion

    }
}
