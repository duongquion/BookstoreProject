using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Book
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public string? AuthorName { get; set; }

    public decimal? IntialPrice { get; set; }

    public decimal? Price { get; set; }

    public string? Language { get; set; }

    public short? PublishingYear { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public int? Amount { get; set; }

    public string? Images { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? MoreImages { get; set; }

    public long GenreId { get; set; }

    public long? PublisherId { get; set; }

    public virtual ICollection<BookDiscount> BookDiscounts { get; set; } = new List<BookDiscount>();

    public virtual ICollection<BookFaverite> BookFaverites { get; set; } = new List<BookFaverite>();

    public virtual ICollection<BookNews> BookNews { get; set; } = new List<BookNews>();

    public virtual ICollection<BookRank> BookRanks { get; set; } = new List<BookRank>();

    public virtual BooksGenre? Genre { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
