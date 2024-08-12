using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class CustomerDiscount
{
    public long Id { get; set; }

    public long? CustomerId { get; set; }

    public long? DiscountId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Discount? Discount { get; set; }
}
