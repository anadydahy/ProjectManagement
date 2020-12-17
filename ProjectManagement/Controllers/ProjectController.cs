using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        private readonly ITicketRepository _ticketRepository;

        public ProjectController(IProjectRepository projectRepository, ITicketRepository ticketRepository)
        {
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddProject(int userId)
        {
            var model = new AddProject() { UserId = userId };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddProject(AddProject model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Project project = new Project() { Name = model.Name, Description = model.Description, Status = model.Status };

            await _projectRepository.UserAddProject(model.UserId, project);

            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {
            if (projectId == 0)
            {
                // here it's mean the request is coming from add ticket not from index
                projectId = (int)TempData["ProjectId"]; // using temp data to pass values throgh RedirectToAction
            }

            var project = await _projectRepository.GetProject(projectId);

            var ticketsWithTheirUsers = await _ticketRepository.GetAllProjectTicketsWithUsers(projectId);

            var projectDetails = new ProjectDetails() { Project = project, Tickets = ticketsWithTheirUsers };

            return View(projectDetails);
        }

    }
}
