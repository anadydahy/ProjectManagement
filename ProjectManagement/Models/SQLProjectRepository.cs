using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagement.Models
{
    public class SQLProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public SQLProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Project> GetProject(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<List<Project>> GetAllProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        public async Task<List<UserProject>> GetUserProjects(string email)
        {
            return await (from user in _context.Users
                          join userProject in _context.UserProjects
                          on user.Id equals userProject.UserId
                          join project in _context.Projects
                          on userProject.ProjectId equals project.Id
                          where user.Email == email
                          select new UserProject
                          {
                              User = user,
                              Project = project,
                              UserId = user.Id,
                              ProjectId = project.Id
                          }).ToListAsync();
        }

        public async Task<List<Ticket>> GetProjectUsersAndRelatedTickets(int projectId)
        {
            return await (from t in _context.Tickets
                          join u in _context.Users
                          on t.UserId equals u.Id
                          where t.ProjectId == projectId
                          select new Ticket
                          {
                              Id = t.Id,
                              Name = t.Name,
                              Description = t.Description,
                              Status = t.Status,
                              UserId = t.UserId,
                              User = t.User
                          }).ToListAsync();

        }

        public async Task<Project> UserAddProject(int userId, Project project)
        {
            var user = await _context.Users.FindAsync(userId);

            if(user == null)
            {
                return null;
            }

            var userProject = new UserProject() { UserId = user.Id, User = user, ProjectId = project.Id, Project = project };

            await _context.UserProjects.AddAsync(userProject);
            await _context.Projects.AddAsync(project);
            await _context.SaveChangesAsync();

            return project;
        }

        public async Task<UserProject> UserEnrollInExistProject(int userId, int projectId)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                return null;
            }

            var project = await _context.Projects.FindAsync(projectId);

            if (project == null)
            {
                return null;
            }

            var userProject = new UserProject() { UserId = user.Id, User = user, ProjectId = project.Id, Project = project };

            await _context.UserProjects.AddAsync(userProject);
            await _context.SaveChangesAsync();

            return userProject;
        }

    }
}
