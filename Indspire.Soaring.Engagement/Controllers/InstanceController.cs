namespace Indspire.Soaring.Engagement.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Indspire.Soaring.Engagement.Data;
    using Indspire.Soaring.Engagement.Database;
    using Indspire.Soaring.Engagement.Extensions;
    using Indspire.Soaring.Engagement.Models;
    using Indspire.Soaring.Engagement.Models.InstanceViewModels;
    using Indspire.Soaring.Engagement.Utils;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize(Roles = RoleNames.Administrator)]
    public class InstanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InstanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var take = pageSize;
            var skip = pageSize * (page - 1);

            var instance = await _context.Instance
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var totalCount = await _context.Instance.CountAsync();

            return View(instance.ToPagedList(totalCount, page, pageSize));
        }

        public IActionResult Create()
        {
            var viewModel = new CreateInstanceViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateInstanceViewModel instanceViewModel)
        {
            var dataUtils = new DataUtils();

            if (ModelState.IsValid)
            {
                var atendee = new Instance();

                if (instanceViewModel != null)
                {
                    atendee.ModifiedDate = atendee.CreatedDate = DateTime.UtcNow;
                    atendee.Description = instanceViewModel.Description;
                    atendee.Name = instanceViewModel.Name;
                }

                _context.Add(atendee);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(instanceViewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Instance.SingleOrDefaultAsync(m => m.InstanceID == id);

            if (user == null)
            {
                return NotFound();
            }

            var viewModel = new EditInstanceViewModel();

            viewModel.DefaultInstance = user.DefaultInstance;
            viewModel.Description = user.Description;
            viewModel.Name = user.Name;
            viewModel.InstanceID = user.InstanceID;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditInstanceViewModel instanceViewModel)
        {
            if (id != instanceViewModel.InstanceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var atendee = _context.Instance
                        .FirstOrDefault(i => i.InstanceID == instanceViewModel.InstanceID);

                    if (atendee != null)
                    {
                        atendee.Name = instanceViewModel.Name;
                        atendee.DefaultInstance = instanceViewModel.DefaultInstance;
                        atendee.Description = instanceViewModel.Description;
                        atendee.ModifiedDate = DateTime.UtcNow;
                    }

                    _context.Update(atendee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstanceExists(instanceViewModel.InstanceID))
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

            return View(instanceViewModel);
        }

        private bool InstanceExists(int id)
        {
            return _context.Instance.Any(e => e.InstanceID == id);
        }
    }
}