using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Competencies
{
    public int Id { get; set; }

    public string? IdNumber { get; set; }

    public string? CompetenceName { get; set; }

    public int? CompetenceType { get; set; }

    public string? CompetenceDetails { get; set; }

    public int? CompetenceParentId { get; set; }

    public int? GroupId { get; set; }

    public bool? CompetenceActive { get; set; }

    public int? CompetenceLevel { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public int? ConceptField { get; set; }

    public virtual ICollection<CompetenciesClass> CompetenciesClasses { get; set; } = new List<CompetenciesClass>();

    public virtual ICollection<CompetenciesConceptField> CompetenciesConceptFields { get; set; } = new List<CompetenciesConceptField>();

    public virtual ICollection<CompetenciesConceptTree> CompetenciesConceptTrees { get; set; } = new List<CompetenciesConceptTree>();

    public virtual ICollection<CompetenciesCross> CompetenciesCrossCompMains { get; set; } = new List<CompetenciesCross>();

    public virtual CompetenciesCross? CompetenciesCrossIdNavigation { get; set; }

    public virtual ICollection<CompetenciesDomain> CompetenciesDomains { get; set; } = new List<CompetenciesDomain>();

    public virtual ICollection<CompetenciesStep> CompetenciesSteps { get; set; } = new List<CompetenciesStep>();
}
