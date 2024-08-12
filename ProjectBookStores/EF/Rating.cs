using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Rating
{
    public long Id { get; set; }

    public byte? Rating1 { get; set; }

    public string? Comment { get; set; }

    public DateTime? CommentDate { get; set; }

    public long? CustomerId { get; set; }

    public long? BookId { get; set; }

    public long? NewId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual BookNews? New { get; set; }
}
