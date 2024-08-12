using ProjectBookStores.EF;

namespace ProjectBookStores.Dao
{
    public class bookGenreDao
    {
        ProjectBookStoreContext db = null;
        public bookGenreDao ()
        {
            db = new ProjectBookStoreContext();
        }

        public IEnumerable<EF.BooksGenre> genreList()
        {
            return db.BooksGenres.Where(x => x.Status == true).ToList();
        }

        public long addGenre(BooksGenre genre)
        {
            db.BooksGenres.Add(genre);
            db.SaveChanges();
            return genre.Id;
        }

        public BooksGenre getGenre(long genId)
        {
            return db.BooksGenres.Find(genId);
        }

        public bool deleteGenre(long id)
        {
            try
            {
                var book = db.BooksGenres.Find(id);
                db.BooksGenres.Remove(book);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}
