using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using test_inventory.Models;
using test_inventory.ViewModel;

namespace test_inventory.Controllers
{
    public class AcountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AcountController
            (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveRegister
            (RegisterUserViewModel UserViewModel)
        {
            if (ModelState.IsValid)
            {
                //Mapping
                ApplicationUser appUser = new ApplicationUser();
                appUser.Address = UserViewModel.Address;
                appUser.UserName = UserViewModel.UserName;
                appUser.PasswordHash = UserViewModel.Password;

                //save database
                IdentityResult result =
                    await userManager.CreateAsync(appUser, UserViewModel.Password);
                if (result.Succeeded)
                {
                    //assign to role
                    await userManager.AddToRoleAsync(appUser, "Admin");
                    //Cookie
                    await signInManager.SignInAsync(appUser, false);
                    return RedirectToAction("Index", "Product");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", UserViewModel);
        }
    }
}
