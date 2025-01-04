using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Models;

namespace RapidRecruit.Controllers
{
    [Authorize(Policy = "BusinessOnly")]
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IAuthorizationService _authorizationService;

        public JobPostingsController(ApplicationDbContext context,
            UserManager<UserAccount> userManager, IAuthorizationService authorizationService)
        {
            _context = context;
            _userManager = userManager;
            _authorizationService = authorizationService;
        }

        // GET: JobPostings
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return View(await _context.JobPosting.Where(JobPosting => JobPosting.UserId == user.Id).Include(j=> j.JobApplications).ToListAsync());
        }

        // GET: JobPostings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _context.JobPosting
                                           .Include(jp => jp.JobApplications)
                                           .ThenInclude(ja => ja.User)
                                           .FirstOrDefaultAsync(m => m.Id == id);
            if (jobPosting == null)
            {
                return NotFound();
            }

            return View(jobPosting);
        }

        // GET: JobPostings/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobPostings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,MinimumSalary,MaximumSalary,Location,EndDate")] JobPosting jobPosting)
        {
            var user = await _userManager.GetUserAsync(User);
            jobPosting.UserId = user.Id;
            jobPosting.CreatedAt = DateTime.Now;
            jobPosting.UpdatedAt = DateTime.Now;
            ModelState.Remove("UserId");
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.Add(jobPosting);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            }
            return View(jobPosting);
        }

        // GET: JobPostings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobPosting = await _context.JobPosting.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, jobPosting, "OwnerPolicy");
            if (authorizationResult.Succeeded)
            {
                return View(jobPosting);

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

        }

        // POST: JobPostings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,MinimumSalary,MaximumSalary,Location,EndDate,CreatedAt,UpdatedAt")] JobPosting jobPosting)
        {
            if (id != jobPosting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobPosting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobPostingExists(jobPosting.Id))
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
            return View(jobPosting);
        }

        // GET: JobPostings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var jobPosting = await _context.JobPosting.FindAsync(id);
            if (jobPosting == null)
            {
                return NotFound();
            }
            var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, jobPosting, "OwnerPolicy");
            if (authorizationResult.Succeeded)
            {
                return View(jobPosting);

            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: JobPostings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobPosting = await _context.JobPosting.FindAsync(id);
            if(jobPosting == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var authorizationResult = await _authorizationService
            .AuthorizeAsync(User, jobPosting, "OwnerPolicy");
            if (authorizationResult.Succeeded)
            {
                _context.JobPosting.Remove(jobPosting);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool JobPostingExists(int id)
        {
            return _context.JobPosting.Any(e => e.Id == id);
        }
    }
}
