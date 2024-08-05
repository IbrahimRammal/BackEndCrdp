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

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<CompetenciesConceptTree> CompetenciesConceptTrees { get; set; } = new List<CompetenciesConceptTree>();

    public virtual ICollection<ConceptTreeClass> ConceptTreeClasses { get; set; } = new List<ConceptTreeClass>();
}
