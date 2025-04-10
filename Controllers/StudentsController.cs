using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Models;
using System.Text;

namespace StudentPerformanceSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Scores)
                .ThenInclude(sc => sc.Subject)
                .FirstOrDefaultAsync(m => m.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,Group,BirthDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student); // Возвращает представление Edit.cshtml с данными студента
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,Group,BirthDate")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);// Возвращает представление с ошибками, если валидация не прошла
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Students.FindAsync(id);
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }

        // Дополнительные методы для расширенной функциональности

        // Подсчет суммы баллов для студента
        public async Task<IActionResult> TotalPoints(int id)
        {
            var student = await _context.Students
                .Include(s => s.Scores)
                .FirstOrDefaultAsync(s => s.StudentId == id);

            if (student == null)
            {
                return NotFound();
            }

            ViewBag.TotalPoints = student.Scores.Sum(s => s.Points);
            return View("Details", student);
        }

        // Топ 5 лучших студентов
        public async Task<IActionResult> TopStudents()
        {
            var topStudents = await _context.Students
                .Include(s => s.Scores)
                .Select(s => new
                {
                    Student = s,
                    TotalPoints = s.Scores.Sum(sc => sc.Points)
                })
                .OrderByDescending(x => x.TotalPoints)
                .Take(5)
                .ToListAsync();

            return View(topStudents.Select(x => x.Student).ToList());
        }

        // Топ 5 худших студентов
        public async Task<IActionResult> WorstStudents()
        {
            var worstStudents = await _context.Students
                .Include(s => s.Scores)
                .Select(s => new
                {
                    Student = s,
                    TotalPoints = s.Scores.Sum(sc => sc.Points)
                })
                .OrderBy(x => x.TotalPoints)
                .Take(5)
                .ToListAsync();

            return View(worstStudents.Select(x => x.Student).ToList());
        }

        // Экспорт списка студентов в текстовый файл
        public async Task<IActionResult> ExportToFile()
        {
            var students = await _context.Students
                .Include(s => s.Scores)
                .ToListAsync();

            var sb = new StringBuilder();
            sb.AppendLine("ID\tФИО\tГруппа\tОбщий балл");

            foreach (var student in students)
            {
                var totalPoints = student.Scores.Sum(s => s.Points);
                sb.AppendLine($"{student.StudentId}\t{student.FullName}\t{student.Group}\t{totalPoints}");
            }

            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain", "students_report.txt");
        }
    }
}
