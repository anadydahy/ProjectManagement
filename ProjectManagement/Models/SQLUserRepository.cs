using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class SQLUserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public SQLUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUser(string email)
        {
            var result = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            return result ?? null;
        }

        public async Task<User> Delete(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            var userTickets = from ticket in _context.Tickets
                              where ticket.User == user
                              select ticket;

            if (userTickets != null)
            {
                _context.Tickets.RemoveRange(userTickets);
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(User userChanges)
        {
            var user = _context.Users.Attach(userChanges);
            user.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return userChanges;
        }
    }
}
