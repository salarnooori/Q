using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Q.Areas.Identity.Data;
using Q.Models;

namespace Q.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly QTaskContext _context;

        public HomeController(ILogger<HomeController> logger, QTaskContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            DateTime date = DateTime.Now;

            return _context.Task != null ?
                          View(_context.Task.Where(t => t.EndTime.Year >= date.Year && t.EndTime.Month >= date.Month && t.EndTime.Day >= date.Day
                                                    && t.StartTime.Year <= date.Year && t.StartTime.Month <= date.Month && t.StartTime.Day <= date.Day).ToList()) :
                          Problem("Entity set 'QTaskContext.Task'  is null.");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}