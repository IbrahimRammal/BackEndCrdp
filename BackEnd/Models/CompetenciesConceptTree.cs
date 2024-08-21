using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesConceptTree
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? ConceptTreeId { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; } = DateTime.Now;

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; } = DateTime.Now;

    public virtual Competencies? CidNavigation { get; set; }

    public virtual ConceptTree? ConceptTree { get; set; }
}
