using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesDomain
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? DomainId { get; set; }

    public virtual Competencies? CidNavigation { get; set; }
}
