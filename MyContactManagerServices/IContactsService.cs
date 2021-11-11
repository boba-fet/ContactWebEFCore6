using ContactWebModels;

namespace MyContactManagerServices
{
    public interface IContactsService
    {
        Task<IList<Contact>> GetAllAsync(string userId);
        Task<Contact?> GetAsync(int id, string userId);
        Task<int> AddOrUpdateAsync(Contact state, string userId);
        Task<int> DeleteAsync(Contact state, string userId);
        Task<int> DeleteAsync(int id, string userId);
        Task<bool> ExistsAsync(int id, string userId);
    }
}
