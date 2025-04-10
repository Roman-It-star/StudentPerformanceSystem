using static System.Formats.Asn1.AsnWriter;

namespace StudentPerformanceSystem.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string Name { get; set; }

        // Навигационное свойство для оценок
        public ICollection<Score> Scores { get; set; } = new List<Score>();
    }
}
