using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Admin
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public string? PassWord { get; set; }

    public string? Email { get; set; }

    public DateTime? CreateDate { get; set; }

    public string? CreateBy { get; set; }

    public bool Status { get; set; }

    public long? PermissionId { get; set; }

    public virtual Permission? Permission { get; set; }
}
