using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CompetenciesCross
{
    public int Id { get; set; }

    public int? CompMainId { get; set; }

    public int? CompSubId { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual Competencies? CompMain { get; set; }

    public virtual Competencies IdNavigation { get; set; } = null!;
}
