using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Models;

namespace StudentPerformanceSystem.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class ScoresController : Controller
    {
        private readonly AppDbContext _context;

        public ScoresController(AppDbContext context)
        {
            _context = context;
        }

        //GET: Scores/Create
        public IActionResult Create(int studentId)
        {
            // Проверяем существование студента
            var student = _context.Students
                .FirstOrDefault(s => s.StudentId == studentId);

            if (student == null)
            {
                return NotFound("Студент не найден");
            }

            // Получаем список предметов
            var subjects = _context.Subjects.ToList();
            if (!subjects.Any())
            {
                return BadRequest("Нет доступных предметов");
            }

            // Создаем модель с предустановленными значениями
            var model = new Score
            {
                StudentId = studentId,
                Date = DateTime.Today
            };

            // Передаем данные в представление
            ViewData["Subjects"] = subjects; // Более надежно, чем ViewBag
            ViewData["StudentName"] = $"{student.LastName} {student.FirstName}";

            return View(model);
        }

        // POST: Scores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ScoreId,StudentId,SubjectId,Points,Date")] Score score)
        {
            if (ModelState.IsValid)
            {
                _context.Add(score);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Students", new { id = score.StudentId });
            }
            ViewBag.StudentId = score.StudentId;
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(score);
        }

        // GET: Scores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Scores.FindAsync(id);
            if (score == null)
            {
                return NotFound();
            }
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(score);
        }

        // POST: Scores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ScoreId,StudentId,SubjectId,Points,Date")] Score score)
        {
            if (id != score.ScoreId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(score);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScoreExists(score.ScoreId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Students", new { id = score.StudentId });
            }
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(score);
        }

        // GET: Scores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var score = await _context.Scores
                .Include(s => s.Student)
                .Include(s => s.Subject)
                .FirstOrDefaultAsync(m => m.ScoreId == id);
            if (score == null)
            {
                return NotFound();
            }

            return View(score);
        }

        // POST: Scores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var score = await _context.Scores.FindAsync(id);
            var studentId = score.StudentId;
            _context.Scores.Remove(score);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Students", new { id = studentId });
        }

        private bool ScoreExists(int id)
        {
            return _context.Scores.Any(e => e.ScoreId == id);
        }
    }
}
