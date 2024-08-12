using ProjectBookStores.EF;

namespace ProjectBookStores.Dao
{
    public class PublisherDao
    {
        ProjectBookStoreContext db = null;
        public PublisherDao()
        {
            db = new ProjectBookStoreContext();
        }
        public List<EF.Publisher> getPublisherList()
        {
            return db.Publishers.Where(x => x.Status == true).ToList();
        }
    }
}
