using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public interface IProjectRepository
    {
        Task<Project> GetProject(int id);

        Task<List<Project>> GetAllProjects();

        Task<List<UserProject>> GetUserProjects(string email);

        Task<List<Ticket>> GetProjectUsersAndRelatedTickets(int projectId);

        Task<Project> UserAddProject(int userId, Project project);

        Task<UserProject> UserEnrollInExistProject(int userId, int projectId);


    }
}
