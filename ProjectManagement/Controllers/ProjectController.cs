using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;


namespace ProjectManagement.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        private readonly ITicketRepository _ticketRepository;

        public ProjectController(IProjectRepository projectRepository, ITicketRepository ticketRepository)
        {
            _projectRepository = projectRepository;
            _ticketRepository = ticketRepository;
        }

        [HttpGet]
        public IActionResult AddProject()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(AddProject model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Project project = new Project() { Name = model.Name, Description = model.Description, Status = model.Status };

            var result = await _projectRepository.UserAddProject(userId, project);

            if (result == null)
            {
                // user id doesn't exist in database
                // this case won't happend unless something wrong with database as we get Id from user claims no chance for error
                return View("~/Views/Project/UserNotFound.cshtml");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            Project project = null;

            if (projectId > -1)
            {
                // user add invalid Id in url (negative)
                project = await _projectRepository.GetProject(projectId);
            }

            if (project == null)
            {
                // no project with such id 
                ViewBag.ProjectId = projectId;
                return View("~/Views/Shared/ProjectWithInvalidId.cshtml");
            }

            var ticketsWithTheirUsers = await _ticketRepository.GetAllProjectTicketsWithUsers(projectId);

            // no need to validate here as if ticketsWithTheirUsers is null won't be displayed in front end
            var projectDetails = new ProjectDetails() { UserId = userId, Project = project, Tickets = ticketsWithTheirUsers };

            return View(projectDetails);
        }

    }
}
