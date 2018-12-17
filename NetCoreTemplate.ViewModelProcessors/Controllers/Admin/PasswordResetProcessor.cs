namespace NetCoreTemplate.ViewModelProcessors.Controllers.Admin
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Transactions;

    using Microsoft.AspNetCore.Http;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.Providers.Interfaces.General;
    using NetCoreTemplate.Services.Interfaces;
    using NetCoreTemplate.Services.Interfaces.General;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.Interfaces.Managers;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModelProcessors.Interfaces;
    using NetCoreTemplate.ViewModels.Controllers.Admin;

    using RazorLight;

    public sealed class PasswordResetProcessor : BaseProcessor<PasswordResetViewModel>
    {
        private readonly IUserProvider userProvider;
        private readonly IBaseService<User> userService;
        private readonly IMailQueueService mailQueueService;
        private readonly IRazorLightEngine razorLightEngine;
        private readonly IReloader<PasswordResetViewModel> reloader;
        private readonly ITranslationManager translationManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PasswordResetProcessor(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
            this.userProvider = serviceContainer.GetService<IUserProvider>();
            this.userService = serviceContainer.GetService<IBaseService<User>>();
            this.mailQueueService = serviceContainer.GetService<IMailQueueService>();
            this.razorLightEngine = serviceContainer.GetService<IRazorLightEngine>();
            this.reloader = serviceContainer.GetServices<IReloader<PasswordResetViewModel>>().First();
            this.translationManager = serviceContainer.GetService<ITranslationManager>();
            this.httpContextAccessor = serviceContainer.GetService<IHttpContextAccessor>();
        }

        public override void Process(PasswordResetViewModel viewModel)
        {
            var user = userProvider.GetUserByEmail(viewModel.Email);

            if (user.IsNullOrDefault())
            {
                return;
            }

            var reloadedViewModel = reloader.Reload(viewModel);
            var subject = translationManager.GetTranslationLabel(LanguageId, "Dashboard:Admin:MailSubject");

            using (var transaction = new TransactionScope(TransactionScopeOption.Required))
            {
                user.ResetToken = RandomToken();
                user.ResetTokenDate = DateTime.Now;

                userService.AddOrUpdate(user);

                reloadedViewModel.Firstname = user.Firstname;
                reloadedViewModel.RootUrl = GetRootUrl();
                reloadedViewModel.Token = user.ResetToken;

                var mailHTML = CreateResetMail(reloadedViewModel).Result;

                mailQueueService.AddNew(
                    user.Email,
                    subject,
                    mailHTML);

                transaction.Complete();
            }
        }

        private static string RandomToken() => StringExtension.GenerateRandomString(32);

        private async Task<string> CreateResetMail(PasswordResetViewModel viewModel)
        {
            return await razorLightEngine.CompileRenderAsync("Mail/ResetMail.cshtml", viewModel);
        }

        private string GetRootUrl()
        {
            var httpContext = httpContextAccessor.HttpContext;

            if (httpContext.Request.IsNullOrDefault() || httpContext.Request.IsNullOrDefault())
            {
                return string.Empty;
            }

            var request = httpContext.Request;
            const string scheme = "https";

            var host = request.Host.Host.ToLower();
            var port = request.Host.Port.GetValueOrDefault(80);

            if (port != 80 && port != 443)
            {
                return $"{scheme}://{host}:{port}";
            }

            return $"{scheme}://{host}";
        }
    }
}
