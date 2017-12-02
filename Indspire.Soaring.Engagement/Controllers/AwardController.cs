namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.AwardViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Indspire.Soaring.Engagement.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AwardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AwardController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Award
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

           
            List<Award> awards = null;
            int totalCount = 0;

            if (string.IsNullOrEmpty(search))
            {
                awards = await _context.Award
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Award.CountAsync();
            }
            else
            {
                awards = await _context.Award
                    .Where(i => i.AwardNumber.Contains(search) || i.Name.Contains(search) || i.Description.Contains(search))
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Award
                    .Where(i => i.AwardNumber.Contains(search) || i.Name.Contains(search) || i.Description.Contains(search))
                    .CountAsync();
            }


            return View(awards.ToPagedList(totalCount, page, pageSize, search));
        }

        public async Task<IActionResult> List()
        {
            return View(await _context.Award.ToListAsync());
        }

        [AllowAnonymous]
        [Route("[controller]/[action]/{awardnumber}")]
        public async Task<IActionResult> Scan(string awardNumber)
        {
            var viewModel = new AwardScanViewModel();

            viewModel.AwardNumber = awardNumber;

            viewModel.HasAwardNumber = !string.IsNullOrWhiteSpace(viewModel.AwardNumber);

            if (viewModel.HasAwardNumber)
            {
                var award = await _context.Award.FirstOrDefaultAsync(i => i.AwardNumber == viewModel.AwardNumber);

                if (award == null)
                {
                    viewModel.AwardNumberInvalid = true;
                }
                else
                {
                    viewModel.Award = award;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Scan")]
        [AllowAnonymous]
        public IActionResult PostScan(string awardNumber)
        {
            var result = string.IsNullOrWhiteSpace(awardNumber)
                ? RedirectToAction("Scan", new { AwardNumber = 0 })
                : RedirectToAction("Scan", new { AwardNumber = awardNumber });

            return result;
        }

        // GET: Award/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award
                .SingleOrDefaultAsync(m => m.AwardID == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateAwardViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAwardViewModel awardViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var award = new Award();

                award.Name = awardViewModel.Name;
                award.Description = awardViewModel.Description;
                award.Points = awardViewModel.Points;
                award.CreatedDate = DateTime.UtcNow;
                award.ModifiedDate = award.CreatedDate;
                award.Deleted = false;
                award.AwardNumber = dataUtils.GenerateNumber();

                _context.Add(award);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(awardViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award.SingleOrDefaultAsync(m => m.AwardID == id);

            if (award == null)
            {
                return NotFound();
            }

            var viewModel = new EditAwardViewModel();

            viewModel.AwardID = award.AwardID;
            viewModel.Description = award.Description;
            viewModel.Name = award.Name;
            viewModel.Points = award.Points;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAwardViewModel awardViewModel)
        {
            if (id != awardViewModel.AwardID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var award = _context.Award
                        .FirstOrDefault(i => i.AwardID == awardViewModel.AwardID);

                    if (award != null)
                    {
                        award.Name = awardViewModel.Name;
                        award.Description = awardViewModel.Description;
                        award.Points = awardViewModel.Points;
                        award.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(award);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AwardExists(awardViewModel.AwardID))
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

            return View(awardViewModel);
        }

        // GET: Award/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var award = await _context.Award
                .SingleOrDefaultAsync(m => m.AwardID == id);
            if (award == null)
            {
                return NotFound();
            }

            return View(award);
        }

        // POST: Award/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var award = await _context.Award.SingleOrDefaultAsync(m => m.AwardID == id);
            _context.Award.Remove(award);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AwardExists(int id)
        {
            return _context.Award.Any(e => e.AwardID == id);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(string AwardNumber, string UserNumber)
        {
            var viewModel = new LogActionJsonViewModel();
            try
            {
               
                //validate 
                var award = await _context.Award.FirstOrDefaultAsync(i => i.AwardNumber == AwardNumber);

                if(award == null)
                {
                    throw new ApplicationException("Award not found.");
                }

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if(user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingAwardLogByThisUser = await _context.AwardLog.FirstOrDefaultAsync(i =>
                                                    i.UserID == user.UserID &&
                                                    i.AwardID == award.AwardID);
                if(existingAwardLogByThisUser != null)
                {
                    throw new ApplicationException($"User has already been Awarded points for this action. User has {PointsUtils.GetPointsForUser(user.UserID, _context)} points.");
                }

                //good to go!

                var awardLog = new AwardLog();
                awardLog.AwardID = award.AwardID;
                awardLog.CreatedDate = DateTime.UtcNow;
                awardLog.ModifiedDate = awardLog.CreatedDate;
                awardLog.Points = award.Points;
                awardLog.UserID = user.UserID;

                _context.Add(awardLog);

                await _context.SaveChangesAsync();

                viewModel.ResponseData.PointsAwarded = awardLog.Points;
                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);
                viewModel.ResponseData.UserNumber = user.UserNumber;
                viewModel.ResponseData.ExternalID = user.ExternalID;

            } catch(Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return new JsonResult(viewModel);

        }
    }
}
