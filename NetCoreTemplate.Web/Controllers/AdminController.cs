namespace NetCoreTemplate.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using NetCoreTemplate.Authentication;
    using NetCoreTemplate.Authentication.Exceptions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModels.Controllers.Admin;
    using NetCoreTemplate.Web.Controllers.Base;
    using NetCoreTemplate.Web.Extensions;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class AdminController : BaseController
    {
        private readonly IAuthenticationClient authentication;
        private readonly ITranslationManager translationManager;

        public AdminController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.authentication = serviceContainer.GetService<IAuthenticationClient>();
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var loader = GetLoader<SignInViewModel>();

            return View("SignIn", loader.Load());
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SignIn(SignInViewModel viewModel)
        {
            var result = ProcessViewModel(
                viewModel,
                async vm =>
                {
                    var validationResult = new ValidationResult<SignInViewModel>();

                    try
                    {
                        var principal = authentication.SignIn(vm.Username, vm.Password);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    }
                    catch (DeactivatedException)
                    {
                        validationResult.AddError(
                            x => x.Username,
                            "Uw account is gedeactiveerd, neem contact op met de systeem beheerder.");

                        ModelState.AddValidationResult(validationResult);

                        throw;
                    }
                    catch (Exception)
                    {
                        validationResult.AddError(
                            x => x.Username,
                            "Uw gebruikersnaam of wachtwoord is verkeerd");

                        validationResult.AddError(
                            x => x.Password,
                            "Uw gebruikersnaam of wachtwoord is verkeerd");

                        ModelState.AddValidationResult(validationResult);

                        throw;
                    }
                },
                () =>
                {
                    AddSuccessNotification(translationManager.GetTranslationLabel(LanguageId, "General", "Notification", "SignedIn"));

                    return RedirectToAction("Index", "Dashboard");
                },
                () =>
                {
                    var reloader = GetReloader<SignInViewModel>();
                    return View("SignIn", reloader.Reload(viewModel));
                });

            return result.Result;
        }

        [HttpGet("signout")]
        public async Task<IActionResult> SignOut()
        {
            await HttpContext.SignOutAsync();

            foreach (var cookie in Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }

            AddSuccessNotification(translationManager.GetTranslationLabel(LanguageId, "General", "Notification", "SignedOut"));

            return RedirectToAction("SignIn");
        }
    }
}
