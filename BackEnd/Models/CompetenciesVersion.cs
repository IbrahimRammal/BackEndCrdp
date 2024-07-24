using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesVersion
{
    public int Id { get; set; }

    public int MainId { get; set; }

    public string? IdNumber { get; set; }

    public string? CompetenceName { get; set; }

    public int? CompetenceType { get; set; }

    public string? CompetenceDetails { get; set; }

    public int? CompetenceParentId { get; set; }

    public bool? CompetenceActive { get; set; }

    public int? CompetenceLevel { get; set; }

    public DateTime? VersionDateCreated { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }
}
