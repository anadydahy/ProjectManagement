using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public interface IUserRepository
    {
        Task<User> GetUser(string userName);

        Task<User> Add(User user);

        Task<User> Update(User userChanges);

        Task<User> Delete(string email);

    }
}
