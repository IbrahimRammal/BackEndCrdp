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

    public virtual Role? Role { get; set; }

    public virtual Service? Service { get; set; }
}
