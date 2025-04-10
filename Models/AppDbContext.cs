using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StudentPerformanceSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Score> Scores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Начальные данные для тестирования
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjectId = 1, Name = "Математика" },
                new Subject { SubjectId = 2, Name = "Физика" },
                new Subject { SubjectId = 3, Name = "Программирование" }
            );
        }


    }
}
