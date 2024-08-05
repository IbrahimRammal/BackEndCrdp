using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConfigConceptTreeLevel
{
    public int Id { get; set; }

    public int? ConceptTreeLevel { get; set; }

    public int? ConceptTreePreviousLevel { get; set; }

    public int? ConceptTreeNextLevel { get; set; }
}
