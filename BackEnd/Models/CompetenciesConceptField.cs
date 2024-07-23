using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesConceptField
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? ConceptFieldId { get; set; }

    public virtual Competency? CidNavigation { get; set; }
}
