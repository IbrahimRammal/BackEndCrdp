using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConceptTree
{
    public int Id { get; set; }

    public string? IdNumber { get; set; }

    public string? ConceptName { get; set; }

    public int? ConceptType { get; set; }

    public int? ConceptDomain { get; set; }

    public int? ConceptField { get; set; }

    public string? ConceptDetails { get; set; }

    public int? ConceptParentId { get; set; }

    public bool? ConceptActive { get; set; }

    public int? ConceptLevel { get; set; }

    public virtual ICollection<CompetenciesConceptTree> CompetenciesConceptTrees { get; set; } = new List<CompetenciesConceptTree>();
}
