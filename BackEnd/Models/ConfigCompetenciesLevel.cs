using System;
using System.Collections.Generic;

namespace BackEnd.Models;

public partial class ConfigCompetenciesLevel
{
    public int? CompetenciesLevel { get; set; }

    public int? CompetenciesPreviousLevel { get; set; }

    public int? CompetenciesNextLevel { get; set; }

    public int Id { get; set; }
}
