using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBookStores.EF;
using ProjectBookStores.ViewModels;
using static System.Reflection.Metadata.BlobBuilder;

namespace ProjectBookStores.Dao
{
    public class bookFavoriteDao
    {
        ProjectBookStoreContext db = null;
        public bookFavoriteDao()
        {
            db = new ProjectBookStoreContext();
        }

        public IEnumerable<favoriteProductViewModel> listFavorites(long id)
        {
            var model = (from a in db.BookFaverites
                         join b in db.Books on a.BookId equals b.Id
                         join c in db.Customers on a.CustomerId equals c.Id
                         join d in db.BooksGenres on b.GenreId equals d.Id
                         where c.Id == id
                         select new favoriteProductViewModel
                         {
                            genreId = d.Id,
                            productId = b.Id,
                            customerId = c.Id,
                            productImages = b.Images,
                            productName = b.Name,
                            productAuthor = b.AuthorName,
                            productDescription = b.Description,
                            productCategory = d.Name,
                            favoriteDate = a.FaverDate,
                            proPrice = b.Price,
                         }).ToList();
            return model;
        }

        public long insertFavorite(BookFaverite favoriteProd)
        {
            db.BookFaverites.Add(favoriteProd);
            db.SaveChanges();
            return favoriteProd.Id;
        }
    }
}
