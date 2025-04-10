using static System.Formats.Asn1.AsnWriter;
using System.ComponentModel.DataAnnotations;

namespace StudentPerformanceSystem.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Имя обязательно")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Фамилия обязательна")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Группа обязательна")]
        [Display(Name = "Группа")]
        public string Group { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        // Навигационное свойство для оценок
        public ICollection<Score> Scores { get; set; } = new List<Score>();

        // Вычисляемое свойство для полного имени
        [Display(Name = "ФИО")]
        public string FullName => $"{LastName} {FirstName}";

    }
}
