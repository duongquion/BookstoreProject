namespace ProjectBookStores.ViewModels
{
    public class ProductViewModel
    {
        public long ProId { get; set; }

        public string? ProName { get; set; }

        public string? AuthorName { get; set; }

        public decimal? IntialPrice { get; set; }

        public decimal? Price { get; set; }

        public string? Language { get; set; }

        public short? PublishingYear { get; set; }

        public string? Description { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? Amount { get; set; }

        public string? Images { get; set; }

        public long GenreId { get; set; }

        public long? PublisherId { get; set; }

        public string? GenresName { get; set; }

        public string? PublisherName { get; set; }
    }
}
