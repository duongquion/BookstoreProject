using ProjectBookStores.EF;
using ProjectBookStores.ViewModels;

namespace ProjectBookStores.Dao
{
    public class OrderDao
    {
        ProjectBookStoreContext db;
        public OrderDao()
        {
            db = new ProjectBookStoreContext();
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return db.Orders.OrderByDescending(x => x.OrderDate).Where(x => x.StatusOrder == false).ToList();
        }

        //Cập nhật trạng thái giao hàng
        public bool UpdateOrderStatus(long orderId, byte statusId)
        {
            try
            {
                var order = db.Orders.Find(orderId);
                if (order != null)
                {
                    order.OrderStatusId = statusId;
                    db.Orders.Update(order);
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool DeleteOrder(long id)
        {
            try
            {
                var order = db.Orders.Find(id);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //Xác nhận đơn hàng
        public bool AccessOrder(long Id)
        {
            try
            {
                var order = db.Orders.Find(Id);
                order.StatusOrder = !order.StatusOrder;
                db.Orders.Update(order);
                db.SaveChanges();
                return true ;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<customerAddressViewModel> GetAddress(long userId)
        {
            var model = (from a in db.Addresses
                         join b in db.Customers on a.CustomerId equals b.Id
                         join c in db.Provinces on a.ProvinceId equals c.Code
                         join d in db.Districts on a.DistrictId equals d.Code
                         join e in db.Wards on a.WardId equals e.Code
                         where a.CustomerId == userId
                         select new customerAddressViewModel
                         {
                             customerId = a.CustomerId,
                             customerEmail = b.Email,
                             customerPhone = b.Phone,
                             detailAddress = a.DetailAddress,
                             Provinces = c.FullName,
                             Districs = d.FullName,
                             Wards = e.FullName
                         }).ToList();
            return model;
        }

        public IEnumerable<detailOrderViewModel> detailOder(long id, string code)
        {
            var model = (from a in db.Addresses
                         join b in db.Customers on a.CustomerId equals b.Id
                         join c in db.Provinces on a.ProvinceId equals c.Code
                         join d in db.Districts on a.DistrictId equals d.Code
                         join e in db.Wards on a.WardId equals e.Code
                         join f in db.Orders on b.Id equals f.CustomerId
                         join g in db.OrderDetails on f.Id equals g.OrderId
                         join j in db.Books on g.BookId equals j.Id
                         join h in db.OrderStatuses on f.OrderStatusId equals h.Id
                         where f.Id == id || f.Code == code
                         select new detailOrderViewModel
                         {
                             customerId = a.CustomerId,
                             customerName = b.FirstName + " " + b.LastName,
                             customerEmail = b.Email,
                             customerPhone = b.Phone,
                             detailAddress = a.DetailAddress,
                             Provinces = c.FullName,
                             Districs = d.FullName,
                             Wards = e.FullName,
                             orderId = f.Id,
                             orderDate = f.OrderDate,
                             orderNote = f.Note,
                             orderPrice = g.TotalPrice,
                             orderCode = f.Code,
                             Status = f.StatusOrder,
                             orderStatus = h.StatusName,
                             productQuantity = g.Quantity,
                             productName = j.Name
                         }).ToList();
            return model;
        }

    }
}