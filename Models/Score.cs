namespace StudentPerformanceSystem.Models
{
    public class Score
    {
        public int ScoreId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int Points { get; set; }
        public DateTime Date { get; set; }

        // Навигационные свойства
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
