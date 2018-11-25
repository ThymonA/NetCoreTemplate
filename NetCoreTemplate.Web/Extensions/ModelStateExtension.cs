namespace NetCoreTemplate.Web.Extensions
{
    using System.Collections.Generic;

    using NetCoreTemplate.SharedKernel.Validation;
    using NetCoreTemplate.ViewModels.Interfaces;

    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public static class ModelStateExtension
    {
        public static void AddValidationResult(
            this ModelStateDictionary modelState,
            ValidationResult validationResult)
        {
            var errors = validationResult.GetAllErrors;

            foreach (var error in errors)
            {
                modelState.AddModelError(error.Key, error.Value);
            }
        }

        public static void RemoveRange(
            this ModelStateDictionary modelState,
            IEnumerable<KeyValuePair<string, ModelStateEntry>> items)
        {
            foreach (var item in items)
            {
                modelState.Remove(item.Key);
            }
        }
    }
}
