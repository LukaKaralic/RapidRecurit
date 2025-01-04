using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Models;

namespace RapidRecruit.Controllers
{
    [Authorize(Policy = "BusinessOnly")]
    public class JobApplicationsController : Controller
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly ApplicationDbContext _context;

        public JobApplicationsController(ApplicationDbContext context, UserManager<UserAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var jobPostingIds = await _context.JobPosting.Where(jp => jp.UserId == user.Id).Select(jp => jp.Id).ToListAsync();
            var jobApplication = await _context.JobApplication.Where(ja => jobPostingIds.Contains(ja.JobPostingId)).Include(j => j.User).Include(ja=> ja.JobPosting).FirstOrDefaultAsync(j => j.Id == id);
            if(jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        public async Task<IActionResult> Review(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var jobPostingIds = await _context.JobPosting.Where(jp => jp.UserId == user.Id).Select(jp => jp.Id).ToListAsync();
            var jobApplication = await _context.JobApplication.Where(ja => jobPostingIds.Contains(ja.JobPostingId)).Include(j => j.User).Include(ja => ja.JobPosting).FirstOrDefaultAsync(j => j.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Review(int id, [Bind("Id,ReviewerNote,ReviewDescision")] JobApplication jobApplicationUpdate)
        {
            var user = await _userManager.GetUserAsync(User);
            var jobPostingIds = await _context.JobPosting.Where(jp => jp.UserId == user.Id).Select(jp => jp.Id).ToListAsync();
            var jobApplication = await _context.JobApplication.Where(ja => jobPostingIds.Contains(ja.JobPostingId)).Include(j => j.User).Include(ja => ja.JobPosting).FirstOrDefaultAsync(j => j.Id == id);

            if (jobApplication == null)
            {
                return NotFound();
            }

            jobApplication.ReviewerNote = jobApplicationUpdate.ReviewerNote;
            jobApplication.ReviewDescision = jobApplicationUpdate.ReviewDescision;
            jobApplication.UpdatedAt = DateTime.Now;

          
            _context.Update(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = jobApplication.Id });
         
        }
    }
}
