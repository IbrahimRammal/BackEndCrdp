namespace BackEnd.Class
{
 
   

    public class CompetenciesInfo
    {
    public string competencieName { get; set; }
    public int CompetenceTypeValue { get; set; }
    public int knowledgeFieldValue { get; set; }
    public List<string> valuePrimayName { get; set; } = new List<string>();
    public List<List<List<int>>> SecondarySelectedConceptTree { get; set; } = new List<List<List<int>>>();
        public List<List<List<string>>> DetailsSecondary { get; set; } = new List<List<List<string>>>();
        public List<List<List<int>>> SecondarySelectedClasses { get; set; } = new List<List<List<int>>>();
        public List<List<string>> selectedNameSecondary { get; set; } = new List<List<string>>();
        
    }
    }



