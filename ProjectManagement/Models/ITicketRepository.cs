using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public interface ITicketRepository
    {
        Task<Ticket> UserAddTicketInExistProject(int userId, int projectId, Ticket ticket);

        Task UserDeleteTicketInExistProject(int userId, int projectId, int ticketId);

        Task<Ticket> UserUpdateTicketInExistProject(int userId, int projectId, Ticket ticketChanges);

        Task<List<Ticket>> GetAllProjectTicketsWithUsers(int projectId);
    }
}
