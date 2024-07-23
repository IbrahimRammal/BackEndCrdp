using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDetails { get; set; }

    public virtual ICollection<RoleService> RoleServices { get; set; } = new List<RoleService>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
