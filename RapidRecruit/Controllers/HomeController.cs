using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<UserAccount> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.JobPosting.Include(j=>j.User).ToListAsync());
        }


        [Route("Home/JobPostings/{id}")]

        public async Task<IActionResult> Job(int id)
        {

            var jobPosting = await _context.JobPosting.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);

        }

        [Route("Home/JobPostings/{id}/Apply")]
        public async Task<IActionResult> Apply(int id)
        {

            var jobPosting = await _context.JobPosting.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);

        }

        [HttpPost]
        [Route("Home/JobPostings/{id}/Apply")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> SubmitApplication(int id, IFormFile resume, string? message)
        {
            foreach (var key in Request.Form.Keys)
            {
                _logger.LogError($"Form key: {key}, Value: {Request.Form[key]}");
            }

            var jobPosting = await _context.JobPosting.Include(j => j.User).FirstOrDefaultAsync(j => j.Id == id);

            if (jobPosting == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            // Handle file upload
            if (resume == null || resume.Length == 0)
            {
                ModelState.AddModelError("resume", "Please upload a resume");
                return View("Apply", jobPosting);
            }

            // Create unique filename
            string uniqueFileName = $"{Guid.NewGuid()}_{resume.FileName}";
            string uploadsFolder = Path.Combine("wwwroot", "uploads", "resumes");
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Ensure directory exists
            Directory.CreateDirectory(uploadsFolder);

            // Save file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await resume.CopyToAsync(fileStream);
            }

            // Create and save job application
            var application = new JobApplication
            {
                UserId = user.Id,
                JobPostingId = id,
                CandidateNote = message,
                ResumeFileName = resume.FileName,
                ResumeFilePath = Path.Combine("uploads", "resumes", uniqueFileName),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.JobApplication.Add(application);
            await _context.SaveChangesAsync();

            // Redirect to appropriate page (e.g., application confirmation or applications list)
            return RedirectToAction("Index");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
