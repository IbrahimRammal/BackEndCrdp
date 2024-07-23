using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class UserRole
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public int? UserId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<UserRolePermission> UserRolePermissions { get; set; } = new List<UserRolePermission>();
}
