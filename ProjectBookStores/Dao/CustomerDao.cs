using ProjectBookStores.EF;

namespace ProjectBookStores.Dao
{
    public class CustomerDao
    {
        ProjectBookStoreContext db = null;
        public CustomerDao()
        {
            db = new ProjectBookStoreContext();
        }

        public Customer GetCustomerById(string email)
        {
            return db.Customers.FirstOrDefault(x => x.Email == email);
        }

        public bool CheckCustomer(string email)
        {
            return db.Customers.Count(x => x.Email == email) > 0;
        }

        public long Insert(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
            return customer.Id;
        }

        public Customer GetCustomerByResetToken (string token)
        {
            return db.Customers.FirstOrDefault(x => x.ResetToken == token);
        }

        public bool Update(Customer customer)
        {
            try
            {
                var user = db.Customers.Find(customer.Id);
                if (user != null)
                {
                    user.PassWord = customer.PassWord;
                    user.ResetToken = customer.ResetToken;
                    user.ResetTokenExpiration = customer.ResetTokenExpiration;
                    db.Customers.Update(user);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int Login (string email, string password)
        {
            var result = db.Customers.FirstOrDefault(x => x.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.PassWord == password)
                {
                    return 1;
                }
                else if (result.Status == false)
                {
                    return -1;
                }
                return -2;
            }
        }
    }
}
