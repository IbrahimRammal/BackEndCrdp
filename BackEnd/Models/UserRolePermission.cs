using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class UserRolePermission
{
    public int Id { get; set; }

    public int? UserRoleId { get; set; }

    public int? Class { get; set; }

    public int? Domain { get; set; }

    public int? ConceptFiled { get; set; }

    public virtual UserRole? UserRole { get; set; }
}
