using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesConceptTree
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? ConceptTreeId { get; set; }

    public virtual Competency? CidNavigation { get; set; }

    public virtual ConceptTree? ConceptTree { get; set; }
}
