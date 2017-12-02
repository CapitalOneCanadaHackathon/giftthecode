namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Utils;
    using Indspire.Soaring.Engagement.ViewModels;
    using Indspire.Soaring.Engagement.Models.RedemptionViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    [Authorize(Roles = RoleNames.Administrator)]
    public class RedemptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RedemptionController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            int page = 1, 
            int pageSize = 20,
            string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);


            List<Redemption> redemptions = null;
            int totalCount = 0;


            if (string.IsNullOrEmpty(search))
            {
                redemptions = await _context.Redemption
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Redemption.CountAsync();
            }
            else
            {
                redemptions = await _context.Redemption
                    .Where(i => i.Name.Contains(search) || i.RedemptionNumber.Contains(search) || i.Description.Contains(search))
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.Redemption
                    .Where(i => i.Name.Contains(search) || i.RedemptionNumber.Contains(search) || i.Description.Contains(search))
                    .CountAsync();
            }

            return View(redemptions.ToPagedList(totalCount, page, pageSize, search));
        }

        // GET: Redemptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption
                .SingleOrDefaultAsync(m => m.RedemptionID == id);
            if (redemption == null)
            {
                return NotFound();
            }

            return View(redemption);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new CreateRedemptionViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRedemptionViewModel redemptionViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var redemption = new Redemption();

                redemption.RedemptionNumber = dataUtils.GenerateNumber();
                redemption.ModifiedDate = redemption.CreatedDate = DateTime.UtcNow;
                redemption.PointsRequired = redemptionViewModel.PointsRequired;
                redemption.Deleted = false;

                if (redemptionViewModel != null)
                {
                    redemption.Name = redemptionViewModel.Name;
                    redemption.PointsRequired = redemptionViewModel.PointsRequired;
                    redemption.Description = redemptionViewModel.Description;
                }

                _context.Add(redemption);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(redemptionViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption
                .SingleOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return NotFound();
            }

            var viewModel = new EditRedemptionViewModel();

            viewModel.RedemptionID = redemption.RedemptionID;
            viewModel.Description = redemption.Description;
            viewModel.Name = redemption.Name;
            viewModel.PointsRequired = redemption.PointsRequired;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, 
            EditRedemptionViewModel redemptionViewModel)
        {
            if (id != redemptionViewModel.RedemptionID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var redemption = await _context.Redemption
                        .FirstOrDefaultAsync(i => i.RedemptionID == redemptionViewModel.RedemptionID);

                    if (redemption != null)
                    {
                        redemption.Name = redemptionViewModel.Name;
                        redemption.Description = redemptionViewModel.Description;
                        redemption.ModifiedDate = DateTime.UtcNow;
                        redemption.PointsRequired = redemptionViewModel.PointsRequired;
                    }

                    _context.Update(redemption);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RedemptionExists(redemptionViewModel.RedemptionID))
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

            return View(redemptionViewModel);
        }

        // GET: Redemptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var redemption = await _context.Redemption
                .SingleOrDefaultAsync(m => m.RedemptionID == id);

            if (redemption == null)
            {
                return NotFound();
            }

            return View(redemption);
        }

        // POST: Redemptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var redemption = await _context.Redemption.SingleOrDefaultAsync(m => m.RedemptionID == id);
            _context.Redemption.Remove(redemption);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RedemptionExists(int id)
        {
            return _context.Redemption.Any(e => e.RedemptionID == id);
        }

        [AllowAnonymous]
        public IActionResult Scan()
        {
            var viewModel = new RedemptionScanViewModel();
            viewModel.RedemptionNumber = $"{Request.Query["RedemptionNumber"]}";
            if (string.IsNullOrEmpty(viewModel.RedemptionNumber))
            {
                viewModel.HasRedemptionNumber = false;
            }
            else
            {
                viewModel.HasRedemptionNumber = true;
            }

            if (viewModel.HasRedemptionNumber)
            {
                var redemption = _context.Redemption.FirstOrDefault(i => i.RedemptionNumber == viewModel.RedemptionNumber);

                if (redemption == null)
                {
                    viewModel.RedemptionNumberInvalid = true;
                }
                else
                {
                    viewModel.Redemption = redemption;
                }
            }
            return View(viewModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LogAction(string RedemptionNumber, string UserNumber)
        {
            var viewModel = new LogRedemptionJsonViewModel();
            try
            {

                //validate 
                var redemption = await _context.Redemption.FirstOrDefaultAsync(i => i.RedemptionNumber == RedemptionNumber);

                if (redemption == null)
                {
                    throw new ApplicationException("Redemption not found.");
                }

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }

                var existingRedemptionByUser = await _context.RedemptionLog.FirstOrDefaultAsync(i =>
                                                    i.UserID == user.UserID &&
                                                    i.RedemptionID == redemption.RedemptionID);
                if (existingRedemptionByUser != null)
                {
                    throw new ApplicationException($"User has already Redeemed this. User has {PointsUtils.GetPointsForUser(user.UserID, _context)} points.");
                }

                //check if we have enough points
                int pointsShort = 0;
                int userPoints = PointsUtils.GetPointsForUser(user.UserID, _context);
                int pointsRequired = redemption.PointsRequired;
                bool hasEnoughPoints = false;

                pointsShort = pointsRequired - userPoints;

                if(pointsShort <= 0)
                {
                    hasEnoughPoints = true;
                    pointsShort = 0;
                }




                if (hasEnoughPoints)
                {
                    //good to go!
                    var redemptionLog = new RedemptionLog();
                    redemptionLog.RedemptionID = redemption.RedemptionID;
                    redemptionLog.CreatedDate = DateTime.UtcNow;
                    redemptionLog.ModifiedDate = redemptionLog.CreatedDate;
                    redemptionLog.UserID = user.UserID;
                    _context.Add(redemptionLog);
                    await _context.SaveChangesAsync();
                }

                

                viewModel.ResponseData.PointsShort = pointsShort;
                viewModel.ResponseData.Success = hasEnoughPoints;
                viewModel.ResponseData.PointsBalance = PointsUtils.GetPointsForUser(user.UserID, _context);
                viewModel.ResponseData.UserNumber = user.UserNumber;
                viewModel.ResponseData.ExternalID = user.ExternalID;

            }
            catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData = null;
            }

            return new JsonResult(viewModel);

        }
    }
}
