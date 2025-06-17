using CoffeeFoodOrder.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace CoffeeFoodOrder
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authManager)
            : base(userManager, authManager) { }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var userManager = context.GetUserManager<ApplicationUserManager>();
            var authManager = context.Authentication;

            if (userManager == null)
                throw new System.Exception("ApplicationUserManager is null");

            return new ApplicationSignInManager(userManager, authManager);
        }
    }
}
