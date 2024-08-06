using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Service
{
    public int Id { get; set; }

    public string? ServiceName { get; set; }

    public string? Clurl { get; set; }

    public string? Svurl { get; set; }

    public string? Dependencies { get; set; }

    public bool? HasChildren { get; set; }

    public string? Parent { get; set; }

    public string? Title { get; set; }

    public int? ParentId { get; set; }

    public virtual ICollection<RoleService> RoleServices { get; set; } = new List<RoleService>();
}
