using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    [Authorize]
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;

        private readonly IProjectRepository _projectRepository;

        public TicketController(ITicketRepository ticketRepository, IProjectRepository projectRepository)
        {
            _ticketRepository = ticketRepository;
            _projectRepository = projectRepository;
        }

        [HttpGet]
        public IActionResult AddTicket(int projectId)
        {
            var model = new AddTicket() { ProjectId = projectId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTicket(AddTicket model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddTicket");
            }

            var projectExist = _projectRepository.GetProject(model.ProjectId);

            if (projectExist == null)
            {
                // no project with such id 
                ViewBag.ProjectId = model.ProjectId;
                return View("~/Views/Shared/ProjectWithInvalidId.cshtml");
            }

            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ticket = new Ticket()
            {
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                UserId = userId,
                ProjectId = model.ProjectId
            };

            await _ticketRepository.UserAddTicketInExistProject(userId, model.ProjectId, ticket);

            return RedirectToAction("ProjectDetails", "Project", new { projectId = model.ProjectId });
        }
    }
}
