using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesVersion
{
    public int Id { get; set; }

    public int MainId { get; set; }

    public string? IdNumber { get; set; }

    public string? CompetencyName { get; set; }

    public int? CompetencyType { get; set; }

    public string? CompetencyDetails { get; set; }

    public int? CompetencyParentId { get; set; }

    public bool? CompetencyActive { get; set; }

    public int? CompetencyLevel { get; set; }

    public DateTime? VersionDateCreated { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }
}
