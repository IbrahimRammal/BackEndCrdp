namespace BackEnd.Class
{
    public class CompetenceLevelDto
    {
        public int CompetenceLevelId { get; set; }
        public string? CompetenceLevelName { get; set; } // New property for level name
        public int Count { get; set; }
        public List<CompetenciesDto> Competencies { get; set; } = new List<CompetenciesDto>();

    }
}