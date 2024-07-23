using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public virtual ICollection<RoleService> RoleServices { get; set; } = new List<RoleService>();
}
