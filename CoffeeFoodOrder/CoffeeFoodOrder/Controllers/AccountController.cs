using CoffeeFoodOrder.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CoffeeFoodOrder.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationSignInManager>();
            private set => _signInManager = value;
        }

        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [HttpGet]
        public ActionResult Register() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                FullName = model.FullName,
                Phone = model.PhoneNumber
            };

            var result = await UserManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);

            return View(model);
        }

        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
            if (result == SignInStatus.Success)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Невірні дані входу");
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        // --- ПРОФІЛЬ ---
        [Authorize]
        [HttpGet]
        public ActionResult UserProfile()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Phone = user.Phone
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserProfile(ProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = UserManager.FindById(User.Identity.GetUserId());

            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            user.FullName = model.FullName;
            user.Phone = model.Phone;

            var result = UserManager.Update(user);
            if (result.Succeeded)
            {
                ViewBag.Status = "Зміни збережено успішно";
            }
            else
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error);
            }

            return View(model);
        }


        [ChildActionOnly]
        public PartialViewResult UserInfoPartial()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
                return PartialView(null);

            var model = new ProfileViewModel
            {
                FullName = user.FullName,
                Phone = user.Phone
            };

            return PartialView("_UserInfoPartial", model);
        }
    }
}
