using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Discount
{
    public long Id { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public decimal? DiscountPercent { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<BookDiscount> BookDiscounts { get; set; } = new List<BookDiscount>();

    public virtual ICollection<CustomerDiscount> CustomerDiscounts { get; set; } = new List<CustomerDiscount>();
}
