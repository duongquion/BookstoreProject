using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class OrderDetail
{
    public long Id { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public decimal? TotalPrice { get; set; }

    public long? OrderId { get; set; }

    public long? BookId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Order? Order { get; set; }
}
