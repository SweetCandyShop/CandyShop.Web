using CandyShop.Core.Models;
using CandyShop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CandyShop.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Index() => View(userManager.Users.ToList());

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserViewModel createUserViewModel)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = createUserViewModel.Email,
                    UserName = createUserViewModel.Email
                };

                await userManager.CreateAsync(user, createUserViewModel.Password);

                if (createUserViewModel.RoleName != string.Empty)
                {
                    await userManager.AddToRoleAsync(user, createUserViewModel.RoleName);
                }

                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string userId)
        {
            User user = await userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index");
        }
    }
}
