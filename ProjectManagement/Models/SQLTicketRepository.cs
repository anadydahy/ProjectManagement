using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class SQLTicketRepository : ITicketRepository
    {
        private readonly AppDbContext _context;

        public SQLTicketRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> UserAddTicketInExistProject(int userId, int projectId, Ticket ticket)
        {
            var user = await _context.Users.FindAsync(userId);
            var project = await _context.Projects.FindAsync(projectId);

            var newticket = new Ticket()
            {
                UserId = user.Id,
                User = user,
                ProjectId = project.Id,
                Project = project,
                Name = ticket.Name,
                Description = ticket.Description,
                Status = ticket.Status
            };

            await _context.Tickets.AddAsync(newticket);
            await _context.SaveChangesAsync();

            return newticket;
        }

        public async Task<List<Ticket>> GetAllProjectTicketsWithUsers(int projectId)
        {
            var usersAndTickets = await (from t in _context.Tickets
                                         join u in _context.Users
                                         on t.UserId equals u.Id
                                         where t.ProjectId == projectId
                                         select new Ticket
                                         {
                                             Id = t.Id,
                                             Name = t.Name,
                                             Description = t.Description,
                                             ProjectId = projectId,
                                             User = u,
                                             UserId = u.Id,
                                             Status = t.Status
                                         }).ToListAsync();
            return usersAndTickets;
        }

        public async Task UserDeleteTicketInExistProject(int userId, int projectId, int ticketId)
        {
            var ticketToDelete = await SearchForATicket(userId, projectId, ticketId);

            _context.Tickets.Remove(ticketToDelete);

            await _context.SaveChangesAsync();
        }

        public async Task<Ticket> UserUpdateTicketInExistProject(int userId, int projectId, Ticket ticketChanges)
        {
            var ticket = _context.Tickets.Attach(ticketChanges);
            ticket.State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return ticketChanges;
        }

        private async Task<Ticket> SearchForATicket(int userId, int projectId, int ticketId)
        {
            return await (from t in _context.Tickets
                          where t.ProjectId == projectId && t.UserId == userId
                          select t).SingleAsync();
        }

    }
}
