namespace Hangman.Data
{
    using System;
    using System.Data.Entity.Validation;
    using System.Text;

    public class FormattedDbEntityValidationException : Exception
    {
        public FormattedDbEntityValidationException(DbEntityValidationException innerException)
            : base(null, innerException)
        {
        }

        public override string Message
        {
            get
            {
                var innerException = this.InnerException as DbEntityValidationException;
                if (innerException != null)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    stringBuilder.AppendLine();
                    stringBuilder.AppendLine();
                    foreach (var entityValidationError in innerException.EntityValidationErrors)
                    {
                        stringBuilder.AppendLine(string.Format(
                            "- Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            entityValidationError.Entry.Entity.GetType().FullName,
                            entityValidationError.Entry.State));
                        foreach (var validationError in entityValidationError.ValidationErrors)
                        {
                            stringBuilder.AppendLine(string.Format(
                                "-- Property: \"{0}\", Value: \"{1}\", Error: \"{2}\"",
                                validationError.PropertyName,
                                entityValidationError.Entry.CurrentValues.GetValue<object>(validationError.PropertyName),
                                validationError.ErrorMessage));
                        }
                    }

                    stringBuilder.AppendLine();

                    return stringBuilder.ToString();
                }

                return base.Message;
            }
        }
    }
}
