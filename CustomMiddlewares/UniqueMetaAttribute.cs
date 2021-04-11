using System;
using FluentBlog.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using FluentBlog.Models;

namespace FluentBlog.CustomMiddlewares
{
    public class UniqueMetaAttribute : ValidationAttribute
    {
        private AppDbContext _context;
        private string _property;

        public UniqueMetaAttribute(string property)
        {
            _property = property;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            _context = (AppDbContext) context.GetService(typeof(AppDbContext));
            bool result = false;

            switch (_property)
            {
                case "Name":
                    result = !_context.Metas.Any(m =>
                        string.Equals(m.Name, value.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    break;
                case "Slug":
                    result = !_context.Metas.Any(m =>
                        string.Equals(m.Slug, value.ToString(), StringComparison.CurrentCultureIgnoreCase));
                    break;
            }

            if (result) return ValidationResult.Success;
            return new ValidationResult("duplicate");
        }
    }
}