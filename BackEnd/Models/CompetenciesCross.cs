using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesCross
{
    public int Id { get; set; }

    public int? CompMainId { get; set; }

    public int? CompSubId { get; set; }

    public virtual Competency? CompMain { get; set; }

    public virtual Competency IdNavigation { get; set; } = null!;
}
