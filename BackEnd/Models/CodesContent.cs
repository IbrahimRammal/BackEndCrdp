using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class CodesContent
{
    public int Id { get; set; }

    public string? CodeContentName { get; set; }

    public string? CodeContentDescription { get; set; }

    public int? CodeId { get; set; }

    public virtual Code? Code { get; set; }
}
