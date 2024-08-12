namespace ProjectBookStores.ViewModels
{
    public class favoriteProductViewModel
    {
        public long genreId { get; set; }
        public long productId { get; set; }
        public long customerId { get; set; }
        public string? productImages { get; set; }
        public string? productName { get; set; }
        public string? productAuthor { get; set; }
        public string? productDescription { get; set; }
        public string? productCategory { get; set; }
        public decimal? proPrice { get; set; }
        public DateOnly? favoriteDate { get; set; }
    }
}
