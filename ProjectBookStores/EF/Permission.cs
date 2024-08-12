using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Permission
{
    public long Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
