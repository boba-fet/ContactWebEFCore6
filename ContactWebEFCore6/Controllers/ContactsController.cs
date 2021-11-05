using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactWebModels;
using MyContactManagerData;
using ContactWebEFCore6.Models;
using Microsoft.Extensions.Caching.Memory;

namespace ContactWebEFCore6.Controllers
{
    public class ContactsController : Controller
    {
        private readonly MyContactManagerDbContext _context;
        private static List<State> _allStates;
        private static SelectList _statesData;
        private IMemoryCache _cache;

        public ContactsController(MyContactManagerDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
            SetAllStatesCachingData();
            _statesData = new SelectList(_allStates, "Id", "Abbreviation");
        }

        private void SetAllStatesCachingData()
        {
            var allStates = new List<State>();
            if (!_cache.TryGetValue(ContactCacheConstants.ALL_STATES, out allStates))
            {
                var allStatesData = Task.Run(() => _context.States.ToListAsync()).Result;

                _cache.Set(ContactCacheConstants.ALL_STATES, allStatesData, TimeSpan.FromDays(1));
                allStates = _cache.Get(ContactCacheConstants.ALL_STATES) as List<State>;
            }
            _allStates = allStates;
        }

        private async Task UpdateStateAndResetModelState(Contact contact)
        {
            ModelState.Clear();
            var state = _allStates.SingleOrDefault(x => x.Id == contact.StateId);
            contact.State = state;
            TryValidateModel(contact);
        }

        // GET: Contacts
        public async Task<IActionResult> Index()
        {
            var contacts = _context.Contacts.Include(c => c.State);
            return View(await contacts.ToListAsync());
        }

        // GET: Contacts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // GET: Contacts/Create
        public IActionResult Create()
        {
            ViewData["StateId"] = _statesData;
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contact contact)
        {
            UpdateStateAndResetModelState(contact);
            if (ModelState.IsValid)
            {
                //hack to get the state hydrated
                var state = await _context.States.SingleOrDefaultAsync(x => x.Id == contact.StateId);
                contact.State = state;
                await _context.Contacts.AddAsync(contact);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StateId"]  = _statesData;
            return View(contact);
        }

        // GET: Contacts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            ViewData["StateId"]  = _statesData;
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Email,PhonePrimary,PhoneSecondary,Birthday,StreetAddress1,StreetAddress2,City,StateId,Zip,UserId")] Contact contact)
        {
            if (id != contact.Id)
            {
                return NotFound();
            }

            UpdateStateAndResetModelState(contact);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Contacts.Update(contact);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contact.Id))
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
            ViewData["StateId"]  = _statesData;
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _context.Contacts
                .Include(c => c.State)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactExists(int id)
        {
            return _context.Contacts.Any(e => e.Id == id);
        }
    }
}
