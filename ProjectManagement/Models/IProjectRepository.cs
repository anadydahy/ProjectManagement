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

        Task UserEnrollInExistProject(int userId, int projectId);

        // only Creator of project "the first user who add a project" can Update it will add role in phase 3
        //Task UpdateProject(Project projectChanges);

        // only Creator of project "the first user who add a project" can delete it will add role in phase 3
        //Task DeleteProjectWithRelatedTickets(int projectId);

    }
}
