using ProjectBookStores.EF;
using ProjectBookStores.Models;
using ProjectBookStores.ViewModels;

namespace ProjectBookStores.Dao
{
    public class AdminDao
    {
        ProjectBookStoreContext db = null;
        
        public AdminDao()
        {
            db = new ProjectBookStoreContext();
        }

        public IEnumerable<AdminViewModel> GetAll(string searchString)
        {
            var query = from admin in db.Admins
                        join permission in db.Permissions on admin.PermissionId equals permission.Id
                        select new AdminViewModel
                        {
                            Id = admin.Id,
                            UserName = admin.UserName,
                            PassWord = admin.PassWord,
                            Email = admin.Email,
                            CreateDate = admin.CreateDate,
                            CreateBy = admin.CreateBy,
                            Status = admin.Status,
                            PermissionName = permission.Name
                        };

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.UserName.Contains(searchString) || x.PermissionName.Contains(searchString));
            }

            return query.OrderByDescending(x => x.CreateDate).ToList();
        }


        public Admin GetAdmin(long id)
        {
            return db.Admins.Find(id);
        }

		public List<GetTimeChartModel> GetOrderChart()
		{
			List<GetTimeChartModel> monthChart = new List<GetTimeChartModel>();

			var query = from a in db.Orders
						join b in db.OrderDetails on a.Id equals b.OrderId
						where a.OrderDate != null
						group b by new { Year = a.OrderDate.Value.Year, Month = a.OrderDate.Value.Month } into g
						select new GetTimeChartModel
						{
							Year = g.Key.Year,
							Month = g.Key.Month,
							NumberOfPrice = g.Sum(x => x.TotalPrice)
						};

			monthChart = query.ToList();
			return monthChart;
		}

		public bool ChangeStatus(long id)
        {
            var admin = db.Admins.Find(id); 
            if (admin == null)
            {
                return false;
            }
            
            admin.Status = !admin.Status;
            db.SaveChanges();
            return admin.Status;
        }

        public long Insert(Admin admin)
        {
            db.Admins.Add(admin);
            db.SaveChanges();
            return admin.Id;
        }

        public bool Update(Admin entity)
        {
            try
            {
                var admin = db.Admins.Find(entity.Id);
                admin.UserName = entity.UserName;
                admin.Email = entity.Email;
                admin.PermissionId = entity.PermissionId;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var admin = db.Admins.Find(id);
                db.Admins.Remove(admin);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        public bool CheckAdmin(string email)
        {
            return db.Admins.Count(x => x.Email == email) > 0;
        }

        public Admin GetAdminById(string emai)
        {
			return db.Admins.SingleOrDefault(x => x.Email == emai);
        }

        public int Login (string email, string password)
        {
            var result = db.Admins.SingleOrDefault(x => x.Email == email);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.Status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.PassWord == password)
                    {
                        return 1;
                    }
                    else
                    {
                        return -2;
                    }
                }
            }
        }
    }
}
