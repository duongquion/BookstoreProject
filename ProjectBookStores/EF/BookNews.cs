using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class BookNews
{
    public long Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreateDate { get; set; }

    public string? CreateBy { get; set; }

    public string? Images { get; set; }

    public bool? Status { get; set; }

    public long? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
