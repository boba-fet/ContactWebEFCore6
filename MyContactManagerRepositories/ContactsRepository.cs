using ContactWebModels;

namespace MyContactManagerRepositories
{
    public class ContactsRepository : IContactsRepository
    {
        public async Task<int> AddOrUpdateAsync(Contact state)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(Contact state)
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Contact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Contact?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
