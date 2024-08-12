namespace ProjectBookStores.ViewModels
{
    public class customerAddressViewModel
    {
        public long customerId { get; set; }
        public string customerEmail { get; set; }
        public int? customerPhone { get; set; }
        public string detailAddress { get; set; }
        public string Provinces { get; set; }
        public string Districs { get; set; }
        public string Wards { get; set; }
    }
}
