using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Competency
{
    public int Id { get; set; }

    public string? IdNumber { get; set; }

    public string? CompetencyName { get; set; }

    public int? CompetencyType { get; set; }

    public string? CompetencyDetails { get; set; }

    public int? CompetencyParentId { get; set; }

    public bool? CompetencyActive { get; set; }

    public int? CompetencyLevel { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<CompetenciesClass> CompetenciesClasses { get; set; } = new List<CompetenciesClass>();

    public virtual ICollection<CompetenciesConceptField> CompetenciesConceptFields { get; set; } = new List<CompetenciesConceptField>();

    public virtual ICollection<CompetenciesConceptTree> CompetenciesConceptTrees { get; set; } = new List<CompetenciesConceptTree>();

    public virtual ICollection<CompetenciesCross> CompetenciesCrossCompMains { get; set; } = new List<CompetenciesCross>();

    public virtual CompetenciesCross? CompetenciesCrossIdNavigation { get; set; }

    public virtual ICollection<CompetenciesDomain> CompetenciesDomains { get; set; } = new List<CompetenciesDomain>();

    public virtual ICollection<CompetenciesStep> CompetenciesSteps { get; set; } = new List<CompetenciesStep>();
}
