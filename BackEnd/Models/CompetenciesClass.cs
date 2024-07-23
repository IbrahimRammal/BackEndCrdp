using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesClass
{
    public int Id { get; set; }

    public int? Cid { get; set; }

    public int? ClassId { get; set; }

    public virtual Competency? CidNavigation { get; set; }
}
