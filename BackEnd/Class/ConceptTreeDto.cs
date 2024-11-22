﻿namespace BackEnd.Class
{
    public class ConceptTreeDto
    {
        public int Id { get; set; }

        public string? IdNumber { get; set; }

        public string? ConceptName { get; set; }

        public int? ConceptType { get; set; }

        public int? ConceptDomain { get; set; }

        public int? ConceptField { get; set; }

        public string? ConceptDetails { get; set; }

        public int? ConceptParentId { get; set; }

        public bool? ConceptActive { get; set; }

        public int? ConceptLevel { get; set; }

        public int? UserCreated { get; set; }

        public DateTime? DateCreated { get; set; } = DateTime.Now;

        public int? UserModified { get; set; }

        public DateTime? DateModified { get; set; } = DateTime.Now;
        public List<string> ClassNames { get; set; } = new List<string>();
    }
}
