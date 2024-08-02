using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConceptTreeClass
{
    public int Id { get; set; }

    public int? Ctid { get; set; }

    public int? ClassId { get; set; }

    public int? UserCreated { get; set; }

    public DateTime? DateCreated { get; set; }

    public int? UserModified { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ConceptTree? Ct { get; set; }
}
