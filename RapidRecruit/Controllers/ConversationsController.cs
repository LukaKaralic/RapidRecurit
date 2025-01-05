using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RapidRecruit.Data;
using RapidRecruit.Models;
using RapidRecruit.Models.Validations;

namespace RapidRecruit.Controllers
{
    public class ConversationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<UserAccount> _userManager;


        public ConversationsController(ApplicationDbContext context, UserManager<UserAccount> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Conversations
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var conversations = await _context.Conversation
                .Include(c => c.Messages)
                .Include(c => c.Candidate)
                .Include(c => c.HiringManager)
                .Where(c => c.CandidateId == userId || c.HiringManagerId == userId)
                .OrderByDescending(c => c.UpdatedAt)
                .ToListAsync();

            return View(conversations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var conversation = await _context.Conversation
                .Include(c => c.Messages)
                .Include(c => c.Candidate)
                .Include(c => c.HiringManager)
                .Include(c => c.JobApplication)
                    .ThenInclude(ja => ja.JobPosting)
                .FirstOrDefaultAsync(c => c.Id == id &&
                    (c.CandidateId == userId || c.HiringManagerId == userId));

            if (conversation == null)
            {
                return NotFound();
            }

            var allConversations = await _context.Conversation
                                                 .Include(c => c.Messages)
                                                 .Include(c => c.Candidate)
                                                 .Include(c => c.HiringManager)
                                                 .Where(c => (c.CandidateId == userId || c.HiringManagerId == userId))
                                                 .OrderByDescending(c => c.UpdatedAt)
                                                 .AsNoTracking()
                                                 .ToListAsync();

            foreach (var conv in allConversations)
            {
                conv.Messages = conv.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(1)
                    .ToList();
            }

            ViewBag.AllConversations = allConversations;

            return View(conversation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(int id, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);
            var conversation = await _context.Conversation
                .FirstOrDefaultAsync(c => c.Id == id &&
                    (c.CandidateId == user.Id || c.HiringManagerId == user.Id));

            if (conversation == null)
            {
                return NotFound();
            }

            var message = new Message
            {
                ConversationId = conversation.Id,
                UserId = user.Id,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            _context.Message.Add(message);

            conversation.UpdatedAt = DateTime.UtcNow;
            _context.Conversation.Update(conversation);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = conversation.Id });
        }

        // GET: Conversations/Create
        [Authorize(Policy = "BusinessOnly")]
        public async Task<IActionResult> Create(int jobApplicationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var jobPostingIds = await _context.JobPosting.Where(jp => jp.UserId == user.Id).Select(jp => jp.Id).ToListAsync();
            var jobApplication = await _context.JobApplication.Where(ja => jobPostingIds.Contains(ja.JobPostingId)).Include(j => j.User).Include(ja => ja.JobPosting).FirstOrDefaultAsync(j => j.Id == jobApplicationId);

            if (jobApplication == null)
            {
                return NotFound();
            }

            var allConversations = await _context.Conversation
                                                .Include(c => c.Messages)
                                                .Include(c => c.Candidate)
                                                .Include(c => c.HiringManager)
                                                .Where(c => (c.HiringManagerId == user.Id))
                                                .OrderByDescending(c => c.UpdatedAt)
                                                .AsNoTracking()
                                                .ToListAsync();

            foreach (var conv in allConversations)
            {
                conv.Messages = conv.Messages
                    .OrderByDescending(m => m.CreatedAt)
                    .Take(1)
                    .ToList();
            }

            ViewBag.AllConversations = allConversations;

            return View(jobApplication);
        }

        // POST: Conversations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Policy = "BusinessOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int jobApplicationId, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return BadRequest();
            }

            var user = await _userManager.GetUserAsync(User);

            var jobPostingIds = await _context.JobPosting
                .Where(jp => jp.UserId == user.Id)
                .Select(jp => jp.Id)
                .ToListAsync();

            var jobApplication = await _context.JobApplication
                .Where(ja => jobPostingIds.Contains(ja.JobPostingId))
                .Include(j => j.User)
                .FirstOrDefaultAsync(j => j.Id == jobApplicationId);

            if (jobApplication == null)
            {
                return NotFound();
            }

            var conversation = new Conversation
            {
                CandidateId = jobApplication.UserId,
                HiringManagerId = user.Id,
                JobApplicationId = jobApplicationId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Conversation.Add(conversation);
                await _context.SaveChangesAsync();

                var message = new Message
                {
                    ConversationId = conversation.Id,
                    UserId = user.Id,
                    Content = content,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.Message.Add(message);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return RedirectToAction("Details", new { id = conversation.Id });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating conversation: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                await transaction.RollbackAsync();
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
