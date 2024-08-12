using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class BookDiscount
{
    public long Id { get; set; }

    public long? BookId { get; set; }

    public long? DiscountId { get; set; }

    public virtual Book? Book { get; set; }

    public virtual Discount? Discount { get; set; }
}
