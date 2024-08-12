namespace ProjectBookStores.Models
{
    public class CartModel
    {
        public long proId { get; set; }
        public long cusId { get; set; }
        public string? proName { get; set; }
        public string? proImage { get; set; }
        public int proQuantity { get; set; }
        public decimal proPrice { get; set; }
        public decimal proDiscount { get; set; }
        public decimal shippingOrder = 20000;
        public decimal finalPrice => proPrice * proQuantity;
    }
}
