using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactWebModels;
using MyContactManagerData;
using Microsoft.Extensions.Caching.Memory;
using ContactWebEFCore6.Models;
using MyContactManagerServices;
using Microsoft.AspNetCore.Authorization;
using ContactWebEFCore6.Data;

namespace ContactWebEFCore6.Controllers
{
    [Authorize(Roles = UserRolesService.ADMIN_ROLE_NAME)]
    public class StatesController : Controller
    {
        private readonly IStatesService _statesService;
        private IMemoryCache _cache;

        public StatesController(IStatesService statesService, IMemoryCache cache)
        {
            _statesService = statesService;
            _cache = cache;
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            var allStates = new List<State>();
            if (!_cache.TryGetValue(ContactCacheConstants.ALL_STATES, out allStates))
            {
                var allStatesData = await _statesService.GetAllAsync() as List<State>;
                
                _cache.Set(ContactCacheConstants.ALL_STATES, allStatesData, TimeSpan.FromDays(1));
                return View(allStatesData);
            }

            return View(allStates);
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation")] State state)
        {
            if (ModelState.IsValid)
            {
                await _statesService.AddOrUpdateAsync(state);
                _cache.Remove(ContactCacheConstants.ALL_STATES);
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Abbreviation")] State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _statesService.AddOrUpdateAsync(state);
                    _cache.Remove(ContactCacheConstants.ALL_STATES);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
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
            return View(state);
        }

        // GET: States/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _statesService.GetAsync((int)id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var state = await _statesService.DeleteAsync(id);
            _cache.Remove(ContactCacheConstants.ALL_STATES);
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(int id)
        {
            return Task.Run(() => _statesService.ExistsAsync(id)).Result;
        }
    }
}
