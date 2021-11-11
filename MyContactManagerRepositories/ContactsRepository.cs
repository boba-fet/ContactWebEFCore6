using Microsoft.EntityFrameworkCore;
using MyContactManagerData;
using ContactWebModels;

namespace MyContactManagerRepositories
{
    public class ContactsRepository : IContactsRepository
    {
        private MyContactManagerDbContext _context;

        public ContactsRepository(MyContactManagerDbContext dbContext)
        {
            _context = dbContext;
        }

        public async Task<IList<Contact>> GetAllAsync(string userId)
        {
            var results = await _context.Contacts.Include(x => x.State)
                                        .AsNoTracking()
                                        .Where(x => x.UserId == userId)
                                        .ToListAsync();
            return results.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
        }

        public async Task<Contact?> GetAsync(int id, string userId)
        {
            var result = await _context.Contacts.Include(x => x.State).AsNoTracking()
                                    .SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            return result;
        }

        public async Task<int> AddOrUpdateAsync(Contact contact, string userId)
        {
            if (contact.Id > 0)
            {
                if (!await ExistsAsync(contact.Id, userId))
                {
                    throw new Exception("Contact Not Found");
                }
                return await Update(contact);
            }

            return await Insert(contact);
        }

        private async Task<int> Insert(Contact contact)
        {
            //contact will come in with State set, but disconnected, need to refresh to existing so won't try to re-insert:
            await GetExistingStateReference(contact);
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return contact.Id;
        }

        private async Task GetExistingStateReference(Contact contact)
        {
            var existingState = await _context.States.SingleOrDefaultAsync(x => x.Id == contact.StateId);
            if (existingState is not null)
            {
                contact.State = existingState;
            }
        }

        private async Task<int> Update(Contact contact)
        {
            var existingContact = await _context.Contacts.SingleOrDefaultAsync(x => x.Id == contact.Id);
            if (existingContact is null) throw new Exception("Contact not found");

            existingContact.Birthday = contact.Birthday;
            existingContact.City = contact.City;
            existingContact.Email = contact.Email;
            existingContact.FirstName = contact.FirstName;
            existingContact.LastName = contact.LastName;
            existingContact.PhonePrimary = contact.PhonePrimary;
            existingContact.PhoneSecondary = contact.PhoneSecondary;
            existingContact.StreetAddress1 = contact.StreetAddress1;
            existingContact.StreetAddress2 = contact.StreetAddress2;
            existingContact.UserId = contact.UserId;
            existingContact.StateId = contact.StateId;
            existingContact.Zip = contact.Zip;

            await _context.SaveChangesAsync();
            return contact.Id;
        }

        public async Task<int> DeleteAsync(Contact contact, string userId)
        {
            return await DeleteAsync(contact.Id, userId);
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            var existingContact = await _context.Contacts
                                                .SingleOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (existingContact is null) throw new Exception("Could not Delete Contact due to unable to find matching state in the database");

            await Task.Run(() => { _context.Contacts.Remove(existingContact); });
            await _context.SaveChangesAsync();
            return id;
        }

        public async Task<bool> ExistsAsync(int id, string userId)
        {
            return await _context.Contacts
                                    .AsNoTracking()
                                    .AnyAsync(x => x.Id == id && x.UserId == userId);
        }
    }
}
