using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class OrderStatus
{
    public byte Id { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
