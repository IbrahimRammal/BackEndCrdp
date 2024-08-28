namespace BackEnd.Class
{
    public class CompetenciesInfo
    {
        public int CompetenceTypeValue { get; set; }
        public string? competencieName { get; set; }
        public string? competencieNumber { get; set; }
        public List<List<List<InfoLayersCompetences>>> Details { get; set; } = new List<List<List<InfoLayersCompetences>>>();
        public string? DetailsCompetence { get; set; }
        public List<List<List<string>>> DetailsSecondary { get; set; } = new List<List<List<string>>>();
        public int knowledgeFieldValue { get; set; }
        public List<List<InfoLayersCompetences>> Secondary { get; set; } = new List<List<InfoLayersCompetences>>();
        public List<List<List<int>>> SecondarySelectedClasses { get; set; } = new List<List<List<int>>>();
        public List<List<List<int>>> SecondarySelectedConceptTree { get; set; } = new List<List<List<int>>>();
        public List<List<List<int>>> SecondarySelectedPublicC { get; set; } = new List<List<List<int>>>();
        public List<List<string>> selectedDetailsSecondary { get; set; } = new List<List<string>>();
        public List<List<string>> selectedNameSecondary { get; set; } = new List<List<string>>();
        public List<string> valueDetailsName { get; set; } = new List<string>();
        public List<string> valuePrimayName { get; set; } = new List<string>();
    }
    }



