using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Order
{
    public long Id { get; set; }

    public DateTime? OrderDate { get; set; }

    public long? CustomerId { get; set; }

    public DateTime? OrderDelivery { get; set; }

    public string? Note { get; set; }

    public bool StatusOrder { get; set; }

    public string? Code { get; set; }

    public byte? OrderStatusId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus? OrderStatus { get; set; }
}
