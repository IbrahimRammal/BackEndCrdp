using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class RoleService
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public int? RoleId { get; set; }

    public bool? CanView { get; set; }

    public bool? CanEdit { get; set; }

    public bool? CanDelete { get; set; }

    public DateTime? DateModified { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserCreated { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Service? Service { get; set; }
}
