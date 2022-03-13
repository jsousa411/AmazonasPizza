using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarterProject.Context.Contexts;
using StarterProject.Context.Contexts.AppContext;

namespace StarterProject.WebAdmin.Controllers
{
    public class AdvertisementConfigsController : Controller
    {
        private readonly AppDbContext _context;

        public AdvertisementConfigsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AdvertisementConfigs
        public async Task<IActionResult> Index()
        {
            return View(await _context.AdvertisementConfig.ToListAsync());
        }

        // GET: AdvertisementConfigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisementConfig = await _context.AdvertisementConfig
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisementConfig == null)
            {
                return NotFound();
            }

            return View(advertisementConfig);
        }

        // GET: AdvertisementConfigs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdvertisementConfigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Type,Active")] AdvertisementConfig advertisementConfig)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advertisementConfig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(advertisementConfig);
        }

        // GET: AdvertisementConfigs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisementConfig = await _context.AdvertisementConfig.FindAsync(id);
            if (advertisementConfig == null)
            {
                return NotFound();
            }
            return View(advertisementConfig);
        }

        // POST: AdvertisementConfigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Type,Active")] AdvertisementConfig advertisementConfig)
        {
            if (id != advertisementConfig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisementConfig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementConfigExists(advertisementConfig.Id))
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
            return View(advertisementConfig);
        }

        // GET: AdvertisementConfigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisementConfig = await _context.AdvertisementConfig
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisementConfig == null)
            {
                return NotFound();
            }

            return View(advertisementConfig);
        }

        // POST: AdvertisementConfigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisementConfig = await _context.AdvertisementConfig.FindAsync(id);
            _context.AdvertisementConfig.Remove(advertisementConfig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementConfigExists(int id)
        {
            return _context.AdvertisementConfig.Any(e => e.Id == id);
        }
    }
}
