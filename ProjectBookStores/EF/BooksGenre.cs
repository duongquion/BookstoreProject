using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class BooksGenre
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
