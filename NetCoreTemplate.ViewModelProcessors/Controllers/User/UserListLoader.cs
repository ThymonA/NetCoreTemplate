namespace NetCoreTemplate.ViewModelProcessors.Controllers.User
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using NetCoreTemplate.DAL.Models.General;
    using NetCoreTemplate.SharedKernel.Expressions;
    using NetCoreTemplate.SharedKernel.Extensions;
    using NetCoreTemplate.SharedKernel.ServiceContainer;
    using NetCoreTemplate.ViewModelProcessors.Base;
    using NetCoreTemplate.ViewModels.Controllers.User;
    using NetCoreTemplate.ViewModels.General;
    using NetCoreTemplate.ViewModels.Models;

    public sealed class UserListLoader : BaseAuthenticationListLoader<User, UserListViewModel, UserViewModel>
    {
        public UserListLoader(IServiceContainer serviceContainer)
            : base(serviceContainer)
        {
        }

        protected override Dictionary<string, List<SortExpression<User>>> CustomOrderByExpressions
            => new Dictionary<string, List<SortExpression<User>>>
            {
                {
                    "Name",
                    new List<SortExpression<User>>
                    {
                        new SortExpression<User>(x => x.Firstname + " " + x.Lastname)
                    }
                },
                {
                    "Active",
                    new List<SortExpression<User>>
                    {
                        new SortExpression<User>(x => x.Active)
                    }
                }
            };

        protected override Expression<Func<User, bool>> Predicate => 
            user => (user.Firstname + " " + user.Lastname).Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                    (user.Lastname + " " + user.Firstname).Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase) ||
                     user.Email.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);

        protected override Expression<Func<User, object>> DefaultOrderBy => user => "Name";

        protected override TranslationSettings TranslationSettings => new TranslationSettings("Dashboard", "User");

        protected override IQueryable<User> BaseQuery => EntityProvider.GetAll()
            .Include(user => user.UserRoles)
            .ThenInclude(userRole => userRole.Role)
            .Where(x => x.Id != 2)
        ;

        protected override UserListViewModel CreateViewModel()
        {
            return new UserListViewModel();
        }

        protected override UserViewModel FillViewModel(User user)
        {
            var viewModel = new UserViewModel
            {
                Id = user.Id,
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                Email = user.Email,
                Active = user.Active,
                ResetToken = user.ResetToken,
                ResetTokenDate = user.ResetTokenDate ?? DateTime.MaxValue
            };

            var roles = user.UserRoles
                .Where(userRole => userRole.Role.Active)
                .Select(userRole => new RoleViewModel
                {
                    Id = userRole.Role.Id,
                    Name = userRole.Role.Name,
                    Active = userRole.Role.Active
                })
                .OrderBy(roleViewModel => roleViewModel.Name)
                .ToList();

            viewModel.Roles = new List<RoleViewModel>();
            viewModel.Roles.AddRange(roles);

            return viewModel;
        }
    }
}
