using ContactWebModels;
using MyContactManagerRepositories;

namespace MyContactManagerServices
{
    public class ContactsService : IContactsService
    {
        private IContactsRepository _contactsRepository;

        public ContactsService(IContactsRepository contactsRepo)
        {
            _contactsRepository = contactsRepo;
        }

        public async Task<IList<Contact>> GetAllAsync(string userId)
        {
            return await _contactsRepository.GetAllAsync(userId);
        }

        public async Task<Contact?> GetAsync(int id, string userId)
        {
            return await _contactsRepository.GetAsync(id, userId);
        }

        public async Task<int> AddOrUpdateAsync(Contact contact, string userId)
        {
            return await _contactsRepository.AddOrUpdateAsync(contact, userId);
        }

        public async Task<int> DeleteAsync(Contact contact, string userId)
        {
            return await _contactsRepository.DeleteAsync(contact, userId);
        }

        public async Task<int> DeleteAsync(int id, string userId)
        {
            return await _contactsRepository.DeleteAsync(id, userId);
        }

        public async Task<bool> ExistsAsync(int id, string userId)
        {
            return await _contactsRepository.ExistsAsync(id, userId);
        }
    }
}
