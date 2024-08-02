using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConfigDomainField
{
    public int Id { get; set; }

    public int? DomainConcept { get; set; }

    public int? FiledConcept { get; set; }
}
