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

        public async Task Add(User user)
        {
            await _context.User.AddAsync(user);

            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(string email)
        {
            var result = await _context.User.SingleOrDefaultAsync(u => u.Email == email);

            return result ?? null;
        }

        //CAUTION : here deleting user with out related Tickets(tasks) better to user DeleteMeAndMyWork()
        public async Task<User> Delete(int id)
        {
            var user = _context.User.Find(id);

            if (user != null)
            {
                _context.User.Remove(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }

        public async Task<User> DeleteMeAndMyWork(string email)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            var userTickets = from ticket in _context.Ticket
                              where ticket.user == user
                              select ticket;

            if (userTickets != null)
            {
                _context.Ticket.RemoveRange(userTickets);
            }

            _context.User.Remove(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<User> Update(User userChanges)
        {
            var user = _context.User.Attach(userChanges);
            user.State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return userChanges;
        }
    }
}
