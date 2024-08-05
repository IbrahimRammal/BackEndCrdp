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

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Competency? CidNavigation { get; set; }
}
