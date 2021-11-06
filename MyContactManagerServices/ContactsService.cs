using ContactWebModels;

namespace MyContactManagerServices
{
    public class ContactsService : IContactsService
    {
        public Task<int> AddOrUpdateAsync(Contact state)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Contact state)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<Contact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Contact?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
