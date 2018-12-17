namespace NetCoreTemplate.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
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

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;

    public class AdminController : BaseController
    {
        private readonly IAuthenticationClient authentication;
        private readonly ITranslationManager translationManager;
        private readonly IBaseProvider<Language> languageProvider;
        private readonly IUserProvider userProvider;

        public AdminController(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.authentication = serviceContainer.GetService<IAuthenticationClient>();
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
            this.languageProvider = serviceContainer.GetService<IBaseProvider<Language>>();
            this.userProvider = serviceContainer.GetService<IUserProvider>();
        }

        [AllowAnonymous]
        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return RedirectToAction("Index", "Dashboard");
        }

        [AllowAnonymous]
        [HttpPost("signin")]
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
                            translationManager.GetTranslationLabel(LanguageId, "Dashboard:SignIn:Deactivated"));

                        ModelState.AddValidationResult(validationResult);

                        throw;
                    }
                    catch (Exception)
                    {
                        validationResult.AddError(
                            x => x.Username,
                            translationManager.GetTranslationLabel(LanguageId, "Dashboard:SignIn:WrongLogin"));

                        validationResult.AddError(
                            x => x.Password,
                            translationManager.GetTranslationLabel(LanguageId, "Dashboard:SignIn:WrongLogin"));

                        ModelState.AddValidationResult(validationResult);

                        throw;
                    }
                },
                () => RedirectToAction("Index", "Dashboard"),
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

            foreach (var cookie in Request.Cookies
                .Where(x => !x.Key.Equals("language",
                    StringComparison.InvariantCultureIgnoreCase)))
            {
                Response.Cookies.Delete(cookie.Key);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        [AllowAnonymous]
        [HttpGet("reset")]
        public IActionResult PasswordReset()
        {
            var loader = GetLoader<PasswordResetViewModel>();

            return View("PasswordReset", loader.Load());
        }

        [AllowAnonymous]
        [HttpPost("reset")]
        public IActionResult PasswordReset(PasswordResetViewModel viewModel)
        {
            return ProcessViewModel(
                viewModel,
                x => RedirectToAction("ResetSuccess", "Admin"),
                x =>
                {
                    var reloader = GetReloader<PasswordResetViewModel>();

                    return View("PasswordReset", reloader.Reload(viewModel));
                });
        }

        [AllowAnonymous]
        [HttpGet("success")]
        public IActionResult ResetSuccess()
        {
            var loader = GetLoader<PasswordResetViewModel>();

            return View("ResetSuccess", loader.Load());
        }

        [AllowAnonymous]
        [HttpGet("expire")]
        public IActionResult ResetExpire()
        {
            var loader = GetLoader<PasswordResetViewModel>();

            return View("ResetExpire", loader.Load());
        }

        [AllowAnonymous]
        [HttpGet("token/")]
        public IActionResult ResetToken()
        {
            return RedirectToAction("ResetExpire", "Admin");
        }

        [AllowAnonymous]
        [HttpGet("token/{token}")]
        public IActionResult ResetToken(string token)
        {
            var user = userProvider.GetUserByToken(token);

            if (user.IsNullOrDefault() ||
                !user.ResetTokenDate.HasValue ||
                user.ResetTokenDate.Value.AddHours(12) < DateTime.Now)
            {
                return RedirectToAction("ResetExpire", "Admin");
            }

            var loader = GetLoader<ResetPasswordViewModel, string>();

            return View("ResetToken", loader.Load(token));
        }

        [AllowAnonymous]
        [HttpPost("token/{token}")]
        public IActionResult ResetToken(ResetPasswordViewModel viewModel)
        {
            return ProcessViewModel(
                viewModel,
                x => RedirectToAction("Index", "Dashboard"),
                x =>
                {
                    var reloader = GetReloader<ResetPasswordViewModel>();

                    return View("ResetToken", reloader.Reload(viewModel));
                });
        }

        [AllowAnonymous]
        [HttpGet("language/{languageCode}/{returnPath}")]
        public IActionResult Language(string languageCode, string returnPath)
        {
            var language = languageProvider
                .GetEntity(x => x.Code.Equals(languageCode, StringComparison.OrdinalIgnoreCase));

            if (language.IsNullOrDefault())
            {
                language = new Language { Id = 1, Code = "NL" };
            }

            Response.Cookies.Append("language", language.Id.ToString());

            var url = WebUtility.UrlDecode(returnPath);

            return Redirect(url);
        }
    }
}
