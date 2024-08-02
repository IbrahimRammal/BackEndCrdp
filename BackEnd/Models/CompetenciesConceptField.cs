using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesConceptField
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? ConceptFieldId { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Competencies? CidNavigation { get; set; }
}
