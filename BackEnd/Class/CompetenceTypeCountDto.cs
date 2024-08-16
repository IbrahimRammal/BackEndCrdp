namespace BackEnd.Class
{
    // By kamel Nazar
    public class CompetenceTypeCountDto
    {
        public int CompetenceType { get; set; }
        public string? CompetenceTypeName { get; set; }
        public int Count { get; set; }
        public List<CompetenceLevelDto> Levels { get; set; } = new List<CompetenceLevelDto>();

    }
}
