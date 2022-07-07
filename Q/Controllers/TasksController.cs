using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Q.Areas.Identity.Data;
using Q.Models;

namespace Q.Controllers
{
    public class TasksController : Controller
    {
        private readonly QTaskContext _context;
        private readonly UserManager<QUser> _userManager;

        public TasksController(QTaskContext context, UserManager<QUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Tasks
        public async Task<IActionResult> Index()
        {
              return _context.Task != null ? 
                          View(await _context.Task.ToListAsync()) :
                          Problem("Entity set 'QTaskContext.Task'  is null.");
        }

        public async Task<IActionResult> FinishedTasks()
        {
            return _context.Task != null ?
                        View(await _context.Task.Where(t => t.isFinish == true).ToListAsync()) :
                        Problem("Entity set 'QTaskContext.Task'  is null.");
        }

        public async Task<IActionResult> UnfinishedTasks()
        {
            return _context.Task != null ?
                        View(await _context.Task.Where(t => t.isFinish == false).ToListAsync()) :
                        Problem("Entity set 'QTaskContext.Task'  is null.");
        }

        public async Task<IActionResult> DateTasks([Bind("date")] DateTime? date)
        {
            if (date != null)
            {
                return RedirectToAction("SearchDateTasks",date);    
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SearchDateTasks(DateTime? date)
        {
            ViewData["date"] = date;
            return View(await _context.Task.Where(t => t.EndTime.Year >= date.Value.Year && t.EndTime.Month >= date.Value.Month && t.EndTime.Day >= date.Value.Day
                                                    && t.StartTime.Year <= date.Value.Year && t.StartTime.Month <= date.Value.Month && t.StartTime.Day <= date.Value.Day).ToListAsync());
        }

        public async Task<IActionResult> CategoryTasks([Bind("category")] int? category)
        {
            if (category != null)
            {
                Category cat = _context.Category.FirstOrDefault(c => c.Id == category);

                return RedirectToAction("SearchCategoryTasks", cat);
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> SearchCategoryTasks(Category cat)
        {
           
            if(cat != null)
            {
                ViewData["category"] = cat.Name;
            }
            else
            {
                ViewData["category"] = "This Category not exist.";
            }
            
            return View(await _context.Task.Where(t => t.CategoryId == cat.Id).ToListAsync());
        }

        // GET: Tasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // GET: Tasks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,CategoryId,period,StartTime,EndTime,N,Details,isFinish,Score")] Models.Task task)
        {
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        // GET: Tasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            if(task.isFinish == false)
            {
                if (task.EndTime > DateTime.UtcNow)
                {
                    return View(task);
                }
                else
                {
                    return RedirectToAction("outOfDate");
                }
                
            }
            else
            {
                return RedirectToAction("notEdit");
            }
        }

        public IActionResult outOfDate()
        {
            return View();
        }

        public IActionResult notEdit()
        {
            return View();
        }

        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,CategoryId,period,StartTime,EndTime,N,Details,isFinish,Score")] Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (task.isFinish == true)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        user.Experience = user.Experience + task.Score;
                        await _userManager.UpdateAsync(user);
                    }
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        // GET: Tasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Task == null)
            {
                return NotFound();
            }

            var task = await _context.Task
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Task == null)
            {
                return Problem("Entity set 'QTaskContext.Task'  is null.");
            }
            var task = await _context.Task.FindAsync(id);
            if (task != null)
            {
                _context.Task.Remove(task);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskExists(int id)
        {
          return (_context.Task?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
