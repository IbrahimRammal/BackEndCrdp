using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class Code
{
    public int Id { get; set; }

    public string? CodeName { get; set; }

    public string? CodeDescription { get; set; }

    public virtual ICollection<CodesContent> CodesContents { get; set; } = new List<CodesContent>();
}
