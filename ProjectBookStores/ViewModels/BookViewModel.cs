using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace ProjectBookStores.ViewModels
{
    public class BookViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string AuthorName { get; set; }

        [Required]
        public decimal IntialPrice { get; set; }

        [Required]
        public decimal Price { get; set; }

        public string Language { get; set; }

        public short PublishingYear { get; set; }

        public string Description { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public long GenreId { get; set; }

        public long? PublisherId { get; set; }

        public string? MainImage { get; set; }

        public string? MoreImages { get; set; }
    }
}
