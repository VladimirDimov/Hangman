namespace Hangman.Web.ViewModels.Games
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    internal class RequiredIfAttribute : RequiredAttribute
    {
        private readonly RequiredAttribute innerAttribute;

        public RequiredIfAttribute(string propertyName, object desiredvalue)
        {
            this.PropertyName = propertyName;
            this.DesiredValue = desiredvalue;
            this.innerAttribute = new RequiredAttribute();
        }

        private string PropertyName { get; set; }

        private object DesiredValue { get; set; }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = this.ErrorMessageString,
                ValidationType = "requiredif",
            };
            rule.ValidationParameters["dependentproperty"] = (context as ViewContext).ViewData.TemplateInfo.GetFullHtmlFieldId(this.PropertyName);
            rule.ValidationParameters["desiredvalue"] =
                this.DesiredValue is bool ?
                this.DesiredValue.ToString().ToLower() :
                this.DesiredValue;

            yield return rule;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var dependentValue = context.ObjectInstance.GetType().GetProperty(this.PropertyName).GetValue(context.ObjectInstance, null);

            if (dependentValue.ToString() == this.DesiredValue.ToString())
            {
                if (!this.innerAttribute.IsValid(value))
                {
                    return new ValidationResult(this.FormatErrorMessage(context.DisplayName), new[] { context.MemberName });
                }
            }

            return ValidationResult.Success;
        }
    }
}
