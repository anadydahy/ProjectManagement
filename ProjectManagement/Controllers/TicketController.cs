using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class TicketController : Controller
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketController(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult AddTicket(int userId, int projectId)
        {
            var model = new AddTicket() { UserId = userId, ProjectId = projectId };

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddTicket(AddTicket model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("AddTicket");
            }

            var ticket = new Ticket()
            {
                Name = model.Name,
                Description = model.Description,
                Status = model.Status,
                UserId = model.UserId,
                ProjectId = model.ProjectId
            };

            await _ticketRepository.UserAddTicketInExistProject(model.UserId, model.ProjectId, ticket);

            TempData["ProjectId"] = model.ProjectId;

            return RedirectToAction("ProjectDetails");
        }
    }
}
