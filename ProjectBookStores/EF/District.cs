using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class District
{
    public string Code { get; set; } = null!;

    public string? FullName { get; set; }

    public string? ProvinceCode { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual Province? ProvinceCodeNavigation { get; set; }

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
