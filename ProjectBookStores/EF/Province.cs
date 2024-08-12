using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Province
{
    public string Code { get; set; } = null!;

    public string? FullName { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
