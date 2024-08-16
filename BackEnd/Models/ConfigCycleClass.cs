using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConfigCycleClass
{
    public int Id { get; set; }

    public int? Cycle { get; set; }

    public int? Class { get; set; }

    public int? PreviousClass { get; set; }

    public int? NextClass { get; set; }
}
