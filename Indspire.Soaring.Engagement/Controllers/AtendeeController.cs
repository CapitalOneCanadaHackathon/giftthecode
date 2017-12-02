namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.AtendeeViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Indspire.Soaring.Engagement.ViewModels;
    using System.Collections.Generic;

    [Authorize(Roles = RoleNames.Administrator)]
    public class AtendeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AtendeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string search = null)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            List<User> users = null;
            int totalCount = 0;

            if (string.IsNullOrEmpty(search))
            {
                users = await _context.User
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.User.CountAsync();
            } else
            {
                users = await _context.User
                    .Where(i => i.UserNumber.Contains(search) || i.ExternalID.Contains(search))
                    .OrderByDescending(i => i.CreatedDate)
                    .Skip(skip)
                    .Take(take)
                    .ToListAsync();

                totalCount = await _context.User
                    .Where(i => i.UserNumber.Contains(search) || i.ExternalID.Contains(search)).CountAsync();
            }

            return View(users.ToPagedList(totalCount, page, pageSize, search));
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var viewModel = new UserDetailsViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var atendee = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (atendee == null)
            {
                return NotFound();
            }

            viewModel.User = atendee;
            viewModel.PointsBalance = PointsUtils.GetPointsForUser(atendee.UserID, _context);

            return View(viewModel);
        }

        public IActionResult Create()
        {
            var viewModel = new CreateAtendeeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateAtendeeViewModel atendeeViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var atendee = new User();

                atendee.ModifiedDate = atendee.CreatedDate = DateTime.UtcNow;
                atendee.Deleted = false;
                atendee.UserNumber = dataUtils.GenerateNumber();

                if (atendeeViewModel != null)
                {
                    atendee.ExternalID = atendeeViewModel.ExternalID;
                }

                _context.Add(atendee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(atendeeViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserID == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditAtendeeViewModel();

            viewModel.ExternalID = user.ExternalID;
            viewModel.UserID = user.UserID;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditAtendeeViewModel atendeeViewModel)
        {
            if (id != atendeeViewModel.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var atendee = _context.User
                        .FirstOrDefault(i => i.UserID == atendeeViewModel.UserID);

                    if (atendee != null)
                    {
                        atendee.ExternalID = atendeeViewModel.ExternalID;
                        atendee.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(atendee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(atendeeViewModel.UserID))
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

            return View(atendeeViewModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atendee = await _context.User
                .SingleOrDefaultAsync(m => m.UserID == id);

            if (atendee == null)
            {
                return NotFound();
            }

            return View(atendee);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var atendee = await _context.User.SingleOrDefaultAsync(m => m.UserID == id);

            _context.User.Remove(atendee);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserID == id);
        }

        [AllowAnonymous]
        [Route("[controller]/scan")]
        public IActionResult Scan()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[controller]/checkbalance")]
        public async Task<IActionResult> CheckBalance(string UserNumber)
        {
            var viewModel = new CheckBalanceJsonViewModel();
            try
            {

                //validate 
                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == UserNumber);

                if (user == null)
                {
                    throw new ApplicationException("User not found.");
                }


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

        public async Task<IActionResult> BulkCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BulkCreateConfirmation(int amount)
        {
            var viewModel = new BulkCreateViewModel();
            viewModel.Amount = amount;
            int usersCreated = 0;
            int maxUsers = 2000;

            try
            {
                if (amount < 1)
                {
                    throw new ApplicationException("Amount to create must be greater than 0");
                }

                if (amount > maxUsers)
                {
                    throw new ApplicationException($"Amount to create must be less than {maxUsers}");
                }


                for(var i = 0; i < amount; i++)
                {
                    var dataUtils = new DataUtils();
                    User user = new User();
                    user.UserNumber = dataUtils.GenerateNumber();
                    user.CreatedDate = DateTime.UtcNow;
                    user.ModifiedDate = user.CreatedDate;   
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    usersCreated++;
                }

                viewModel.AmountCreated = usersCreated;
                viewModel.Success = true;

                return View("BulkCreateSuccess", viewModel);
            }catch (Exception ex)
            {
                viewModel.Success = false;
                viewModel.AmountCreated = usersCreated;
                viewModel.ErrorMessage = $"An error occurred while trying to Bulk Create Users. Error: {ex.Message}.";
                return View("BulkCreateFailed", viewModel);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SetExternalID(string userNumber, string externalID)
        {
            var viewModel = new SetUserExternalIDJsonViewModel();
            try
            {

                var user = await _context.User.FirstOrDefaultAsync(i => i.UserNumber == userNumber);

                if(user == null)
                {
                    throw new ApplicationException("User not found");
                }

                user.ExternalID = externalID;

                _context.Update(user);

                await _context.SaveChangesAsync();

                viewModel.ResponseData.Success = true;

            } catch (Exception ex)
            {
                viewModel.ErrorMessage = ex.Message;
                viewModel.ResponseData.Success = false;
            }

            return Json(viewModel);
        }

    }
}
