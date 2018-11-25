namespace NetCoreTemplate.SharedKernel.Validation
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    using NetCoreTemplate.SharedKernel.Extensions;

    public class ValidationResult<TModel> : ValidationResult
        where TModel : class
    {
        public void AddError<TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            string value) => AddError(expression, value, true);

        public void AddError<TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            string value,
            bool overrideExisting)
            => AddError(expression.GetNameFromExpression(), value, overrideExisting);
    }

    public class ValidationResult
    {
        private Dictionary<string, string> Errors { get; } = new Dictionary<string, string>();

        public void AddError<TModel, TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            string value) => AddError(expression, value, true);

        public void AddError<TModel, TProperty>(
            Expression<Func<TModel, TProperty>> expression,
            string value,
            bool overrideExisting)
            => AddError(expression.GetNameFromExpression(), value, overrideExisting);

        public void AddError(string key, string value)
            => AddError(key, value, false);

        public void AddError(string key, string value, bool overrideExisting)
        {
            if (Errors.ContainsKey(key) && overrideExisting)
            {
                Errors.Remove(key);
                Errors.Add(key, value);
            }
            else if (Errors.ContainsKey(key))
            {
            }
            else
            {
                Errors.Add(key, value);
            }
        }

        public void RemoveError(string key)
        {
            if (Errors.ContainsKey(key))
            {
                Errors.Remove(key);
            }
        }

        public int NumberOfErrors => Errors.Count;

        public Dictionary<string, string> GetAllErrors => Errors;

        public string GetError(string key) => Errors.ContainsKey(key) ? Errors[key] : string.Empty;
    }
}
