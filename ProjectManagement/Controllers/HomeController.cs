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

        public async Task<IActionResult> Index()
        {
            var indexViewModel = new IndexViewModel();

            var projects = await _projectRepository.GetAllProjects();

            if (projects.Count == 0)
            {
                return View("~/Views/Home/noProjectsInSystem.cshtml");
            }

            indexViewModel.Projects = projects;

            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (!isAuthenticated)
            {
                indexViewModel.UserProjects = null;

                return View(indexViewModel);
            }

            string userEmail = User.Claims?.ToList().FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))?.Value;

            var userProjects = await _projectRepository.GetUserProjects(userEmail);

            indexViewModel.UserProjects = userProjects;

            return View(indexViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserEnrollProject(int userId, int projectId)
        {
            await _projectRepository.UserEnrollInExistProject(userId, projectId);

            return RedirectToAction("Index");
        }

    }
}
