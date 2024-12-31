using System.ComponentModel.DataAnnotations;

namespace Conference.Client.Validators
{
    public class SimpleValidator
    {
        public static bool IsValid(object instance)
            => Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), null, true);
    }
}
