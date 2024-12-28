using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Models;
using System.Diagnostics;

namespace RapidRecruit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.JobPosting.Include(j=>j.User).ToListAsync());
        }

        public async Task<IActionResult> Job(int id)
        {

            var jobPosting = await _context.JobPosting.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
