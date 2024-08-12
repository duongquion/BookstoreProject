using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Publisher
{
    public long Id { get; set; }

    public string? PublisherName { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
