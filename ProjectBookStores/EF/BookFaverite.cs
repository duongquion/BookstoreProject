using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class BookFaverite
{
    public long Id { get; set; }

    public DateOnly? FaverDate { get; set; }

    public long? CustomerId { get; set; }

    public long? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Customer? Customer { get; set; }
}
