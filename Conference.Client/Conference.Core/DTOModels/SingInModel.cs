using System.ComponentModel.DataAnnotations;

namespace Conference.Core.DTOModels
{
    public class SingInModel
    {
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Length should be between 3 and 40 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [StringLength(40, MinimumLength = 8, ErrorMessage = "Length should be between 8 and 40 characters")]
        public string Password { get; set; }

        public SingInModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
