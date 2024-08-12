using System;
using System.Collections.Generic;

namespace ProjectBookStores.EF;

public partial class Customer
{
    public long Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? PassWord { get; set; }

    public string? Images { get; set; }

    public string? Email { get; set; }

    public DateTime? CreateDate { get; set; }

    public bool? Status { get; set; }

    public int? Phone { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public long? PermissionId { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetTokenExpiration { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<BookFaverite> BookFaverites { get; set; } = new List<BookFaverite>();

    public virtual ICollection<CustomerDiscount> CustomerDiscounts { get; set; } = new List<CustomerDiscount>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Permission? Permission { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}
