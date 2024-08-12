namespace ProjectBookStores.ViewModels
{
    public class detailOrderViewModel
    {
        public long customerId { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public int? customerPhone { get; set; }
        public string detailAddress { get; set; }
        public string Provinces { get; set; }
        public string Districs { get; set; }
        public string Wards { get; set; }
        public long orderId { get; set; }
        public string orderCode { get; set; }
        public DateTime? orderDate { get; set; }
        public string orderNote { get; set; }
        public int? productQuantity { get; set; }
        public decimal? orderPrice { get; set; }
        public string orderStatus { get; set; }
        public bool  Status{ get; set; }
        public string productName { get; set; }
    }
}
