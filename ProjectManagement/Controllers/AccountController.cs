using ProjectManagement.Models;
using ProjectManagement.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginView model)
        {
            // to know if user authentiatied
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                return RedirectToAction("Home/Index");
            }

            var user = await _userRepository.GetUser(model.Email);

            if (user == null)
            {
                // user not found but not revealing this info to client security reasons
                ModelState.AddModelError("", "Invailed User Name or Password"); // 
                return View();
            }

            bool verified = BCrypt.Net.BCrypt.Verify(model.Password, user.Password);

            if (!verified)
            {
                // user found but type wrong password
                ModelState.AddModelError("", "Invailed User Name or Password"); // 
                return View();
            }

            await SignMeIn(user.Id, user.Email, model.RememberMe);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            bool isAuthenticated = User.Identity.IsAuthenticated;

            if (isAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInput model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var isUserExist = await _userRepository.GetUser(model.Email);


            if (isUserExist != null)
            {
                // user name (Email) must be unique
                ModelState.AddModelError("", "User is already exists"); // 
                return View();
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

            var newUser = new User() { Name = model.Name, Email = model.Email, Password = passwordHash };

            newUser = await _userRepository.Add(newUser);

            await SignMeIn(newUser.Id, newUser.Email);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string email)
        {
            var user = await _userRepository.GetUser(email);

            var model = new RegisterInput()
            {
                Name = user.Name,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(RegisterInput model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUser(model.Email);

                user.Name = model.Name;

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                user.Password = passwordHash;

                await _userRepository.Update(user);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Delete(string email)
        {
            await _userRepository.Delete(email);

            await LogOut();

            return RedirectToAction("Index", "Home");
        }

        private async Task SignMeIn(int userId, string email, bool isPresistent = false)
        {
            var userClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier ,userId.ToString()),
                new Claim(ClaimTypes.Email, email)
            };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = isPresistent
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal, authProperties);
        }
    }
}
