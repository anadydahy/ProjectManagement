using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName);

        //IAsyncEnumerable<User> GetAllUsers();
        Task Add(User user);
        Task<User> Update(User userChanges);

        //CAUTION: here deleting user with out related Tickets(tasks) better to user DeleteMeAndMyWork()
        Task<User> Delete(int id);
        Task<User> DeleteMeAndMyWork(string email);
    }
}
