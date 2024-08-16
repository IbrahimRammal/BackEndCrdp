using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Fname { get; set; }

    public string? Mname { get; set; }

    public string? Lname { get; set; }

    public string? PhoneNb { get; set; }

    public string? Email { get; set; }

    public string Password { get; set; } = null!;

    public string? Details { get; set; }

    public bool? UserStatus { get; set; }

    public int? WorkGroup { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
