using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class BookRank
{
    public long Id { get; set; }

    public int? Ranking { get; set; }

    public long? BookId { get; set; }

    public string? Banner { get; set; }

    public virtual Book? Book { get; set; }
}
