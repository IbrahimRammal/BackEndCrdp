using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesStep
{
    public int Id { get; set; }

    public string? Step { get; set; }

    public int? StepUserId { get; set; }

    public string? StepComment { get; set; }

    public int? StepStatus { get; set; }

    public DateTime? StepDate { get; set; }

    public int? Cid { get; set; }

    public virtual Competency? CidNavigation { get; set; }
}
