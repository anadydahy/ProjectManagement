using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProjectRepository _projectRepository;

        public HomeController(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public IActionResult nady()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var indexView = new IndexView();

            var projects = await _projectRepository.GetAllProjects();

            if (projects.Count == 0)
            {
                return View("~/Views/Home/NoProjectsInSystem.cshtml");
            }

            indexView.Projects = projects;

            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                indexView.UserProjects = null;

                return View(indexView);
            }

            string userEmail = User.Claims?.ToList().FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

            var userProjects = await _projectRepository.GetUserProjects(userEmail);

            indexView.UserProjects = userProjects;

            return View(indexView);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserEnrollProject(int projectId)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var result = await _projectRepository.UserEnrollInExistProject(userId, projectId);

            if(result == null)
            {
                // here something wrong with database as userId is getteing from claims and project id comes from method action in button
                return View("~/Views/shared/Error.cshtml");
            }

            return RedirectToAction("Index");
        }

    }
}
