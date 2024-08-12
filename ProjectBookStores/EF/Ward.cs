using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Ward
{
    public string Code { get; set; } = null!;

    public string? FullName { get; set; }

    public string? DistrictCode { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual District? DistrictCodeNavigation { get; set; }
}
