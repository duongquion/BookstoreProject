using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Address
{
    public long Id { get; set; }

    public string? DetailAddress { get; set; }

    public string? ProvinceId { get; set; }

    public string? DistrictId { get; set; }

    public string? WardId { get; set; }

    public long CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual District? District { get; set; }

    public virtual Province? Province { get; set; }

    public virtual Ward? Ward { get; set; }
}
